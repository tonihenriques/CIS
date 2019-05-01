using GISModel.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GISModel.DTO
{
    public class OperacaoCollection : ICollection<Operacao>
    {
        private readonly HashSet<Operacao> _operacoes;

        public OperacaoCollection()
        {
            _operacoes = new HashSet<Operacao>();
        }

        public int Count => _operacoes.Count;

        public bool IsReadOnly => false;

        public void Add(Operacao item)
        {
            _operacoes.Add(item);
        }

        public void Clear()
        {
            _operacoes.Clear();
        }

        public bool Contains(Operacao item)
        {
            return _operacoes.Contains(item);
        }

        public void CopyTo(Operacao[] array, int arrayIndex)
        {
            _operacoes.CopyTo(array, arrayIndex);
        }

        public IEnumerator<Operacao> GetEnumerator()
        {
            return _operacoes.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _operacoes.GetEnumerator();
        }

        public bool Remove(Operacao item)
        {
            if (!_operacoes.Contains(item))
                return false;

            _operacoes.Remove(item);

            return true;
        }
    }
}
