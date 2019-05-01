using GISCore.Business.Abstract;
using GISModel.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GISCore.Business.Concrete
{
    public class FornecedorBusiness : BaseBusiness<Fornecedor>, IFornecedorBusiness
    {

        //public override void Inserir(Fornecedor fornecedor)
        //{
        //    if (Consulta.Any(u => string.IsNullOrEmpty(u.UsuarioExclusao) && u.CNPJ.Equals(fornecedor.CNPJ.Trim())))
        //        throw new InvalidOperationException("Não é possível inserir o fornecedor, pois já existe um registrado com este CNPJ.");

        //    fornecedor.UniqueKey = Guid.NewGuid().ToString();

        //    base.Inserir(fornecedor);
        //}

        public override void Alterar(Fornecedor entidade)
        {

            Fornecedor tempFornecedor = Consulta.FirstOrDefault(p => string.IsNullOrEmpty(p.UsuarioExclusao) && p.UniqueKey.Equals(entidade.UniqueKey));
            if (tempFornecedor == null)
            {
                throw new Exception("Não foi possível encontrar o fornecedor através da identificação única.");
            }
            else
            {
                tempFornecedor.UniqueKey= entidade.UniqueKey;
                tempFornecedor.NomeFantasia=entidade.NomeFantasia;
                tempFornecedor.CNPJ = entidade.CNPJ;
                //tempFornecedor.DataInclusao = entidade.DataInclusao;
                tempFornecedor.UsuarioExclusao = entidade.UsuarioExclusao;
                base.Alterar(tempFornecedor);

                //entidade.UniqueKey = tempFornecedor.UniqueKey;
                //entidade.UsuarioExclusao = string.Empty;
                //base.Inserir(entidade);

                //tempFornecedor.Numero = fornecedor.Numero;
                //tempFornecedor.Nome = fornecedor.Nome;
                //tempFornecedor.Descricao = fornecedor.Descricao;
                //tempFornecedor.Telefone = fornecedor.Telefone;
                //tempFornecedor.Email = fornecedor.Email;
                //tempFornecedor.Endereco = fornecedor.Endereco;
                //tempFornecedor.UsuarioExclusao = fornecedor.UsuarioExclusao;
                //tempFornecedor.DataExclusao = fornecedor.DataExclusao;
                //base.Alterar(tempFornecedor);

            }

        }

    }




}

