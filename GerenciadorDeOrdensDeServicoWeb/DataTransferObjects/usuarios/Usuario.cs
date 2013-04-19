using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GerenciadorDeOrdensDeServicoWeb.DataTransferObjects.usuarios {
	public class Usuario {

		private UInt32 _codigo;
		private String _nome;
		private String _senha;

		public Usuario() {
			_codigo = 0;
			_nome = String.Empty;
			_senha = String.Empty;
		}

		public Usuario(UInt32 codigo, String nome, String senha) {
			_codigo = codigo;
			_nome = nome;
			_senha = senha;
		}

		public UInt32 codigo {
			get { return _codigo; }
			set { _codigo = value; }
		}
		public String nome {
			get { return _nome; }
			set { _nome = value; }
		}
		public String senha {
			get { return _senha; }
			set { _senha = value; }
		}
	}
}