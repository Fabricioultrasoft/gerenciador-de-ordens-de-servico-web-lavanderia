using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GerenciadorDeOrdensDeServicoWeb.DataTransferObjects.enderecos {
	public class Logradouro {

		private UInt32 _codigo;
		private String _nome;
		private String _cep;

		private Bairro _bairro;
		private TipoDeLogradouro _tipoDeLogradouro;

		public Logradouro() {
			_codigo = 0;
			_nome = String.Empty;
			_cep = String.Empty;

			_bairro = new Bairro();
			_tipoDeLogradouro = new TipoDeLogradouro();
		}

		public Logradouro( UInt32 codLogradouro, String nomLogradouro, String cepLogradouro ) {
			codigo = codLogradouro;
			nome = nomLogradouro;
			cep = cepLogradouro;

			_bairro = new Bairro();
			_tipoDeLogradouro = new TipoDeLogradouro();
		}

		public UInt32 codigo {
			get { return _codigo; }
			set { _codigo = value; }
		}

		public String nome {
			get { return _nome; }
			set { _nome = value; }
		}

		public String cep {
			get { return _cep; }
			set { _cep = value; }
		}

		public Bairro bairro {
			get { return _bairro; }
			set { _bairro = value; }
		}

		public TipoDeLogradouro tipoDeLogradouro {
			get { return _tipoDeLogradouro; }
			set { _tipoDeLogradouro = value; }
		}
	}
}