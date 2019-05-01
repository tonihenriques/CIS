using GISModel.DTO.Account;
using GISModel.Entidades;

namespace GISWeb.Infraestrutura.Provider.Abstract
{
    public interface ICustomAuthorizationProvider
    {

        AutenticacaoModel UsuarioAutenticado { get; }

        bool Autenticado { get; }

        void LogIn(AutenticacaoModel autenticacaoModel, out string msgErro);

        void LogOut();

    }
}