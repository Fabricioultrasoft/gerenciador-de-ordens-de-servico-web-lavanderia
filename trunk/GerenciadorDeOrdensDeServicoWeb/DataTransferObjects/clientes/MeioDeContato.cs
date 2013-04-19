using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GerenciadorDeOrdensDeServicoWeb.DataTransferObjects.clientes {

	//public enum enumTipoDeContato : int {
	//    tel_residencial = 1,
	//    tel_comercial = 2,
	//    celular = 3,
	//    email = 4,
	//    radio = 5,
	//    outros = 6
	//}

	
	public class MeioDeContato {

		private UInt32 _codigo;
		private TipoDeContato _tipoDeContato;
		private String _contato;
		private UInt32 _intContato;
		private String _descricao;

		

		public MeioDeContato() {
			_codigo = 0;
			_tipoDeContato = new TipoDeContato();
			_contato = String.Empty;
			_intContato = 0;
			_descricao = String.Empty;
		}

		public UInt32 codigo {
			get { return _codigo; }
			set { _codigo = value; }
		}
		public TipoDeContato tipoDeContato {
			get { return _tipoDeContato; }
			set { _tipoDeContato = value; }
		}
		public String contato {
			get { return _contato; }
			set { _contato = value; }
		}
		public UInt32 intContato {
			get { return _intContato; }
			set { _intContato = value; }
		}
		public String descricao {
			get { return _descricao; }
			set { _descricao = value; }
		}
	}
}