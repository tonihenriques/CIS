﻿using GISCore.Business.Abstract;
using GISHelpers.Utils;
using GISModel.DTO.Account;
using GISModel.DTO.Permissoes;
using GISModel.Entidades;
using GISModel.Enums;
using Ninject;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.ServiceModel;
using System.Text;

namespace GISCore.Business.Concrete
{
    public class UsuarioBusiness : BaseBusiness<Usuario>, IUsuarioBusiness
    {

        [Inject]
        public IEmpresaBusiness EmpresaBusiness { get; set; }

        [Inject]
        public IUsuarioPerfilBusiness UsuarioPerfilBusiness { get; set; }

        [Inject]
        public IPerfilBusiness PerfilBusiness { get; set; }
        
        [Inject]
        public IDepartamentoBusiness DepartamentoBusiness { get; set; }


        public override void Inserir(Usuario usuario)
        {
            if (Consulta.Any(u => u.Login.Equals(usuario.Login)))
                throw new InvalidOperationException("Não é possível inserir usuário com o mesmo login.");

            usuario.UniqueKey = Guid.NewGuid().ToString();

            base.Inserir(usuario);

            //Enviar e-mail
            //if (usuario.TipoDeAcesso.Equals(TipoDeAcesso.AD))
            //{
            //    EnviarEmailParaUsuarioRecemCriadoAD(usuario);
            //}
            //else
            //{
            //    EnviarEmailParaUsuarioRecemCriadoSistema(usuario);
            //}
        }

        public override void Alterar(Usuario entidade)
        {
            if (Consulta.Any(u => string.IsNullOrEmpty(u.UsuarioExclusao) && !u.UniqueKey.Equals(entidade.UniqueKey) && u.Login.Equals(entidade.Login.Trim())))
                throw new InvalidOperationException("Não é possível atualizar este usuário, pois o Login já está sendo usado por outro usuário.");

            Usuario temp = Consulta.FirstOrDefault(p => p.UniqueKey.Equals(entidade.UniqueKey));
            if (temp == null)
            {
                throw new Exception("Não foi possível encontrar o usuário através da identificação fornecida.");
            }
            else
            {
                temp.UsuarioExclusao = entidade.UsuarioInclusao;
                temp.DataExclusao = DateTime.Now;
                base.Alterar(temp);

                entidade.Senha = temp.Senha;
                entidade.DataInclusao = temp.DataInclusao;
                base.Inserir(entidade);
            }

            
        }

        public void DefinirSenha(NovaSenhaViewModel novaSenhaViewModel)
        {
            Usuario oUsuario = Consulta.FirstOrDefault(p => string.IsNullOrEmpty(p.UsuarioExclusao) && p.ID.Equals(novaSenhaViewModel.IDUsuario));
            if (oUsuario == null)
            {
                throw new Exception("Não foi possível localizar o usuário através da identificação. Solicite um novo acesso.");
            }
            else
            {
                //oUsuario.Senha = CreateHashFromPassword(novaSenhaViewModel.NovaSenha);
                //Alterar(oUsuario);


                oUsuario.UsuarioExclusao = oUsuario.Login;
                Terminar(oUsuario);


                Usuario oUser = new Usuario()
                {
                    Email = oUsuario.Email,
                    Login = oUsuario.Login,
                    Nome = oUsuario.Nome,
                    Senha = CreateHashFromPassword(novaSenhaViewModel.ConfirmarNovaSenha),
                    TipoDeAcesso = oUsuario.TipoDeAcesso,
                    UKDepartamento = oUsuario.UKDepartamento,
                    UKEmpresa = oUsuario.UKEmpresa,
                    UniqueKey = oUsuario.UniqueKey
                };
                base.Inserir(oUser);


                EnviarEmailParaUsuarioSenhaAlterada(oUsuario);
            }
        }

        public void SolicitarAcesso(string email)
        {
            List<Usuario> listaUsuarios = Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao) && p.Email.ToLower().Equals(email.ToLower())).ToList();
            if (listaUsuarios.Count() > 1 || listaUsuarios.Count() < 1)
            {
                listaUsuarios = Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao) && p.Login.ToLower().Equals(email.ToLower())).ToList();
                if (listaUsuarios.Count() > 1 || listaUsuarios.Count() < 1)
                {
                    throw new Exception("Não foi possível localizar este usuário no sistema através do e-mail. Tente novamente ou procure o Administrador.");
                }
            }

            EnviarEmailParaUsuarioSolicacaoAcesso(listaUsuarios[0]);
        }

        public AutenticacaoModel ValidarCredenciais(AutenticacaoModel autenticacaoModel)
        {
            autenticacaoModel.Login = autenticacaoModel.Login.Trim().ToUpper();

            //Buscar usuário sem validar senha, para poder determinar se a validação da senha será com AD ou com a senha interna do GIS
            List<Usuario> lUsuarios = Consulta.Where(u => string.IsNullOrEmpty(u.UsuarioExclusao) &&
                                                     (u.Login.Equals(autenticacaoModel.Login) || u.Email.Equals(autenticacaoModel.Login))).ToList();

            if (lUsuarios.Count == 0)
            {
                throw new Exception("Não foi possível identificar o seu cadastro. Entre em contato com o Administrador do sistema.");
            }
            else if (lUsuarios.Count > 1)
            {
                throw new Exception("Não foi possível identificar o seu cadastro.");
            }
            else
            {
                if (lUsuarios[0].TipoDeAcesso == TipoDeAcesso.AD)
                {
                    throw new Exception("Login através do AD não implementado. Favor acionar o administrador para maiores detalhes.");
                }
                else
                {
                    //Login, validando a senha interna no CIS
                    string IDUsuario = lUsuarios[0].UniqueKey;

                    string senhaCrypt = CreateHashFromPassword(autenticacaoModel.Senha);

                    Usuario oUsuario = Consulta.FirstOrDefault(p => p.UniqueKey.Equals(IDUsuario) && p.Senha.Equals(senhaCrypt));
                    //Usuario oUsuario = Consulta.FirstOrDefault(p => p.UniqueKey.Equals(IDUsuario));
                    if (oUsuario != null)
                    {
                        List<VMPermissao> listapermissoes = new List<VMPermissao>();

                        listapermissoes.AddRange(from usuarioperfil in UsuarioPerfilBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList()
                                                 join perfil in PerfilBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList() on usuarioperfil.UKPerfil equals perfil.UniqueKey
                                                 where usuarioperfil.UKUsuario.Equals(IDUsuario)
                                                 select new VMPermissao { Perfil = perfil.Nome });

                        if (listapermissoes.Count == 0)
                        {
                            throw new Exception("O usuário não possui permissão para acessar o sistema. Entre em contato com o Administrador.");
                        }

                        return new AutenticacaoModel() { UniqueKey = IDUsuario, Login = oUsuario.Login, Nome = oUsuario.Nome, Email = oUsuario.Email, TipoDeAcesso = lUsuarios[0].TipoDeAcesso, Permissoes = listapermissoes };
                    }
                    else
                    {
                        throw new Exception("Login ou senha incorretos.");
                    }
                }
            }

        }

       


        #region E-mails

        private void EnviarEmailParaUsuarioSolicacaoAcesso(Usuario usuario)
            {
                string sRemetente = ConfigurationManager.AppSettings["Web:Remetente"];
                string sSMTP = ConfigurationManager.AppSettings["Web:SMTP"];

                MailMessage mail = new MailMessage(sRemetente, usuario.Email);

                string PrimeiroNome = GISHelpers.Utils.Severino.PrimeiraMaiusculaTodasPalavras(usuario.Nome);
                if (PrimeiroNome.Contains(" "))
                    PrimeiroNome = PrimeiroNome.Substring(0, PrimeiroNome.IndexOf(" "));

                mail.Subject = PrimeiroNome + ", este é o link para redinir sua senha";
                mail.Body = "<html style=\"font-family: Verdana; font-size: 11pt;\"><body>Olá, " + PrimeiroNome + ".";
                mail.Body += "<br /><br />";
                mail.Body += "<span style=\"color: #222;\">Redefina sua senha para começar novamente.";
                mail.Body += "<br /><br />";

                string sLink = "http://localhost:26717/Conta/DefinirNovaSenha/" + WebUtility.UrlEncode(Criptografador.Criptografar(usuario.UniqueKey + "#" + DateTime.Now.ToString("yyyyMMdd"), 1)).Replace("%", "_@");

                mail.Body += "Para alterar sua senha do GiS, clique <a href=\"" + sLink + "\">aqui</a> ou cole o seguinte link no seu navegador.";
                mail.Body += "<br /><br />";
                mail.Body += sLink;
                mail.Body += "<br /><br />";
                mail.Body += "O link é válido por 24 horas, portanto, utilize-o imediatamente.";
                mail.Body += "<br /><br />";
                mail.Body += "Obrigado por utilizar o GiS!<br />";
                mail.Body += "<strong>Gestão Inteligente da Segurança</strong>";
                mail.Body += "</span>";
                mail.Body += "<br /><br />";
                mail.Body += "<span style=\"color: #aaa; font-size: 10pt; font-style: italic;\">Mensagem enviada automaticamente, favor não responder este email.</span>";
                mail.Body += "</body></html>";

                mail.IsBodyHtml = true;
                mail.BodyEncoding = Encoding.UTF8;


                SmtpClient smtpClient = new SmtpClient(sSMTP, 587);

                smtpClient.Credentials = new System.Net.NetworkCredential()
                {
                    UserName = ConfigurationManager.AppSettings["Web:Remetente"],
                    Password = "sesmtajt"
                };

                smtpClient.EnableSsl = true;
                System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate (object s,
                        System.Security.Cryptography.X509Certificates.X509Certificate certificate,
                        System.Security.Cryptography.X509Certificates.X509Chain chain,
                        System.Net.Security.SslPolicyErrors sslPolicyErrors)
                {
                    return true;
                };

                smtpClient.Send(mail);

            }

            private void EnviarEmailParaUsuarioSenhaAlterada(Usuario usuario)
            {
                string sRemetente = ConfigurationManager.AppSettings["Web:Remetente"];
                string sSMTP = ConfigurationManager.AppSettings["Web:SMTP"];

                MailMessage mail = new MailMessage(sRemetente, usuario.Email);

                string PrimeiroNome = GISHelpers.Utils.Severino.PrimeiraMaiusculaTodasPalavras(usuario.Nome);
                if (PrimeiroNome.Contains(" "))
                    PrimeiroNome = PrimeiroNome.Substring(0, PrimeiroNome.IndexOf(" "));

                mail.Subject = PrimeiroNome + ", sua senha foi redefinida.";

                mail.Body = "<html style=\"font-family: Verdana; font-size: 11pt;\"><body>Olá, " + PrimeiroNome + ".";
                mail.Body += "<br /><br />";
                mail.Body += "<span style=\"color: #222;\">Você redefiniu sua senha do GiS.";
                mail.Body += "<br /><br />";
                mail.Body += "Obrigado por utilizar o GiS!<br />";
                mail.Body += "<strong>Gestão Inteligente da Segurança</strong>";
                mail.Body += "</span>";
                mail.Body += "<br /><br />";
                mail.Body += "<span style=\"color: #aaa; font-size: 10pt; font-style: italic;\">Mensagem enviada automaticamente, favor não responder este email.</span>";
                mail.Body += "</body></html>";

                mail.IsBodyHtml = true;
                mail.BodyEncoding = Encoding.UTF8;


                SmtpClient smtpClient = new SmtpClient(sSMTP, 587);

                smtpClient.Credentials = new System.Net.NetworkCredential()
                {
                    UserName = ConfigurationManager.AppSettings["Web:Remetente"],
                    Password = "sesmtajt"
                };

                smtpClient.EnableSsl = true;
                System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate (object s,
                        System.Security.Cryptography.X509Certificates.X509Certificate certificate,
                        System.Security.Cryptography.X509Certificates.X509Chain chain,
                        System.Net.Security.SslPolicyErrors sslPolicyErrors)
                {
                    return true;
                };

                smtpClient.Send(mail);

            }

            private void EnviarEmailParaUsuarioRecemCriadoSistema(Usuario usuario)
            {
                string sRemetente = ConfigurationManager.AppSettings["Web:Remetente"];
                string sSMTP = ConfigurationManager.AppSettings["Web:SMTP"];

                MailMessage mail = new MailMessage(sRemetente, usuario.Email);

                string PrimeiroNome = GISHelpers.Utils.Severino.PrimeiraMaiusculaTodasPalavras(usuario.Nome);
                if (PrimeiroNome.Contains(" "))
                    PrimeiroNome = PrimeiroNome.Substring(0, PrimeiroNome.IndexOf(" "));

                mail.Subject = PrimeiroNome + ", seja bem-vindo!";
                mail.Body = "<html style=\"font-family: Verdana; font-size: 11pt;\"><body>Olá, " + PrimeiroNome + ";";
                mail.Body += "<br /><br />";

                string NomeUsuarioInclusao = usuario.UsuarioInclusao;
                Usuario uInclusao = Consulta.FirstOrDefault(p => string.IsNullOrEmpty(p.UsuarioExclusao) && p.Login.Equals(usuario.UsuarioInclusao));
                if (uInclusao != null && !string.IsNullOrEmpty(uInclusao.Nome))
                    NomeUsuarioInclusao = uInclusao.Nome;


                string sLink = "http://localhost:26717/Conta/DefinirNovaSenha/" + WebUtility.UrlEncode(GISHelpers.Utils.Criptografador.Criptografar(usuario.UniqueKey + "#" + DateTime.Now.ToString("yyyyMMdd"), 1)).Replace("%", "_@");

                mail.Body += "Você foi cadastrado no sistema GiS - Gestão Inteligente da Segurança pelo " + GISHelpers.Utils.Severino.PrimeiraMaiusculaTodasPalavras(NomeUsuarioInclusao) + ".";
                mail.Body += "<br /><br />";
                mail.Body += "Clique <a href=\"" + sLink + "\">aqui</a> para ativar sua conta ou cole o seguinte link no seu navegador.";
                mail.Body += "<br /><br />";
                mail.Body += sLink;
                mail.Body += "<br /><br />";
                mail.Body += "Obrigado por utilizar o GiS!<br />";
                mail.Body += "<strong>Gestão Inteligente da Segurança</strong>";
                mail.Body += "<br /><br />";
                mail.Body += "<span style=\"color: #ccc; font-style: italic;\">Mensagem enviada automaticamente, favor não responder este email.</span>";
                mail.Body += "</body></html>";

                mail.IsBodyHtml = true;
                mail.BodyEncoding = Encoding.UTF8;

                SmtpClient smtpClient = new SmtpClient(sSMTP, 587);

                smtpClient.Credentials = new System.Net.NetworkCredential()
                {
                    UserName = ConfigurationManager.AppSettings["Web:Remetente"],
                    Password = "sesmtajt"
                };

                smtpClient.EnableSsl = true;
                System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate (object s,
                        System.Security.Cryptography.X509Certificates.X509Certificate certificate,
                        System.Security.Cryptography.X509Certificates.X509Chain chain,
                        System.Net.Security.SslPolicyErrors sslPolicyErrors)
                {
                    return true;
                };

                smtpClient.Send(mail);

            }

            private void EnviarEmailParaUsuarioRecemCriadoAD(Usuario usuario)
            {
                string sRemetente = ConfigurationManager.AppSettings["Web:Remetente"];
                string sSMTP = ConfigurationManager.AppSettings["Web:SMTP"];

                MailMessage mail = new MailMessage(sRemetente, usuario.Email);

                string PrimeiroNome = GISHelpers.Utils.Severino.PrimeiraMaiusculaTodasPalavras(usuario.Nome);
                if (PrimeiroNome.Contains(" "))
                    PrimeiroNome = PrimeiroNome.Substring(0, PrimeiroNome.IndexOf(" "));

                mail.Subject = PrimeiroNome + ", seja bem-vindo!";
                mail.Body = "<html style=\"font-family: Verdana; font-size: 11pt;\"><body>Olá, " + PrimeiroNome + ".";
                mail.Body += "<br /><br />";

                string NomeUsuarioInclusao = usuario.UsuarioInclusao;
                Usuario uInclusao = Consulta.FirstOrDefault(p => string.IsNullOrEmpty(p.UsuarioExclusao) && p.Login.Equals(usuario.UsuarioInclusao));
                if (uInclusao != null && !string.IsNullOrEmpty(uInclusao.Nome))
                    NomeUsuarioInclusao = uInclusao.Nome;

                string sLink = "http://localhost:26717/";

                mail.Body += "Você foi cadastrado no sistema GiS - Gestão Inteligente da Segurança pelo " + NomeUsuarioInclusao + ".";
                mail.Body += "<br /><br />";
                mail.Body += "Clique <a href=\"" + sLink + "\">aqui</a> para acessar a sua conta ou cole o seguinte link no seu navegador.";
                mail.Body += "<br /><br />";
                mail.Body += sLink;
                mail.Body += "<br /><br />";
                mail.Body += "Obrigado por utilizar o GiS!<br />";
                mail.Body += "<strong>Gestão Inteligente da Segurança</strong>";
                mail.Body += "<br /><br />";
                mail.Body += "<span style=\"color: #ccc; font-style: italic;\">Mensagem enviada automaticamente, favor não responder este email.</span>";
                mail.Body += "</body></html>";

                mail.IsBodyHtml = true;
                mail.BodyEncoding = Encoding.UTF8;

                SmtpClient smtpClient = new SmtpClient(sSMTP, 587);

                smtpClient.Credentials = new System.Net.NetworkCredential()
                {
                    UserName = ConfigurationManager.AppSettings["Web:Remetente"],
                    Password = "sesmtajt"
                };

                smtpClient.EnableSsl = true;
                System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate (object s,
                        System.Security.Cryptography.X509Certificates.X509Certificate certificate,
                        System.Security.Cryptography.X509Certificates.X509Chain chain,
                        System.Net.Security.SslPolicyErrors sslPolicyErrors)
                {
                    return true;
                };

                smtpClient.Send(mail);

            }

        #endregion
        
        #region Senhas

        [ComVisible(false)]
            public string CreateHashFromPassword(string pstrOriginalPassword)
            {
                if (string.IsNullOrEmpty(pstrOriginalPassword))
                    return string.Empty;

                string str3 = ConvertToHashedString(pstrOriginalPassword).Substring(0, 5);
                byte[] bytes = Encoding.UTF8.GetBytes(pstrOriginalPassword + str3);
                HashAlgorithm lobjHash = new MD5CryptoServiceProvider();
                return Convert.ToBase64String(lobjHash.ComputeHash(bytes));
            }

            [ComVisible(false)]
            private string ConvertToHashedString(string pstrOriginal)
            {
                byte[] bytes = Encoding.UTF8.GetBytes(pstrOriginal);
                HashAlgorithm lobjHash = new MD5CryptoServiceProvider();
                return Convert.ToBase64String(lobjHash.ComputeHash(bytes));
            }

        #endregion

        #region Avatar
        
            public byte[] RecuperarAvatar(string login)
            {
                try
                {
                    WCF_Suporte.SuporteClient WCFSuporte = new WCF_Suporte.SuporteClient();
                    return WCFSuporte.BuscarFotoPerfil(new WCF_Suporte.DadosUsuario()
                    {
                        Login = login
                    });
                }
                catch (FaultException<WCF_Suporte.FaultSTARSServices> ex)
                {
                    throw new Exception(ex.Detail.Detalhes);
                }
            }

            public void SalvarAvatar(string login, string imageStringBase64)
            {
                try
                {
                    WCF_Suporte.SuporteClient WCFSuporte = new WCF_Suporte.SuporteClient();
                    WCFSuporte.SalvarFotoPerfil(new WCF_Suporte.DadosUsuario()
                    {
                        Login = login
                    }, imageStringBase64);
                }
                catch (FaultException<WCF_Suporte.FaultSTARSServices> ex)
                {
                    throw new Exception(ex.Detail.Detalhes);
                }
            }
        
        #endregion

    }
}
