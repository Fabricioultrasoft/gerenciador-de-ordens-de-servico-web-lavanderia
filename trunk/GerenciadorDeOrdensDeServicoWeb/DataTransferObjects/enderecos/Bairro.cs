using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GerenciadorDeOrdensDeServicoWeb.DataTransferObjects.enderecos {
	public class Bairro {

		private UInt32 _codigo;
		private String _nome;

		private Cidade _cidade;

		public Bairro() {
			_codigo = 0;
			_nome = String.Empty;

			_cidade = new Cidade();
		}

		public Bairro( UInt32 codBairro, String nomBairro ) {
			codigo = codBairro;
			nome = nomBairro;

			_cidade = new Cidade();
		}

		public UInt32 codigo {
			get { return _codigo; }
			set { _codigo = value; }
		}

		public String nome {
			get { return _nome; }
			set { _nome = value; }
		}

		public Cidade cidade {
			get { return _cidade; }
			set { _cidade = value; }
		}
	}
}