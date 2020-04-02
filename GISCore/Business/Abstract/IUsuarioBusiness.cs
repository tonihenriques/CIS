using GISModel.DTO.Account;
using GISModel.Entidades;

namespace GISCore.Business.Abstract
{
    public interface IUsuarioBusiness : IBaseBusiness<Usuario>
    {

        AutenticacaoModel ValidarCredenciais(AutenticacaoModel autenticacaoModel);

        byte[] RecuperarAvatar(string login);

        void SalvarAvatar(string login, string imageStringBase64);

        void DefinirSenha(NovaSenhaViewModel novaSenhaViewModel);

        string CreateHashFromPassword(string pstrOriginalPassword);

    }
}
