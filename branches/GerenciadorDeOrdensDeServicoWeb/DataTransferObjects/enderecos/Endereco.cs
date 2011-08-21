using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GerenciadorDeOrdensDeServicoWeb.DataTransferObjects.enderecos {
	public class Endereco {

		private UInt32 _codigo;
		private Pais _pais;
		private Estado _estado;
		private Cidade _cidade;
		private Bairro _bairro;
		private Logradouro _logradouro;
		private String _complemento;
		private String _pontoDeReferencia;
		private UInt32 _numero;

		public Endereco() {
			_codigo = 0;
			_pais = new Pais();
			_estado = new Estado();
			_cidade = new Cidade();
			_bairro = new Bairro();
			_logradouro = new Logradouro();
			_complemento = String.Empty;
			_pontoDeReferencia = String.Empty;
			_numero = 0;
		}

		public UInt32 codigo {
			get { return _codigo; }
			set { _codigo = value; }
		}
		public Pais pais {
			get { return _pais; }
			set { _pais = value; }
		}
		public Estado estado {
			get { return _estado; }
			set { _estado = value; }
		}
		public Cidade cidade {
			get { return _cidade; }
			set { _cidade = value; }
		}
		public Bairro bairro {
			get { return _bairro; }
			set { _bairro = value; }
		}
		public Logradouro logradouro {
			get { return _logradouro; }
			set { _logradouro = value; }
		}
		public String complemento {
			get { return _complemento; }
			set { _complemento = value; }
		}
		public String pontoDeReferencia {
			get { return _pontoDeReferencia; }
			set { _pontoDeReferencia = value; }
		}
		public UInt32 numero {
			get { return _numero; }
			set { _numero = value; }
		}
	}
}