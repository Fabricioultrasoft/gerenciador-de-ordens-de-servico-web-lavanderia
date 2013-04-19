using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GerenciadorDeOrdensDeServicoWeb.DataTransferObjects.tapetes {
	public class Tapete {

		private UInt32 _codigo;
		private String _nome;
		private String _descricao;
		private bool _ativo;

		public Tapete() {
			_codigo = 0;
			_nome = String.Empty;
			_descricao = String.Empty;
			_ativo = false;
		}

		public Tapete( UInt32 codigo, String nome, String descricao, bool ativo ) {
			this.codigo = codigo;
			this.nome = nome;
			this.descricao = descricao;
			this.ativo = ativo;
		}

		public UInt32 codigo {
			get { return _codigo; }
			set { _codigo = value; }
		}

		public String nome {
			get { return _nome; }
			set { _nome = value; }
		}

		public String descricao {
			get { return _descricao; }
			set { _descricao = value; }
		}

		public bool ativo {
			get { return _ativo; }
			set { _ativo = value; }
		}
	}
}