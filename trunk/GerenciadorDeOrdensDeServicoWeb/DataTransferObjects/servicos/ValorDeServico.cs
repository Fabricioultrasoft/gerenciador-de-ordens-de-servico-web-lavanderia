using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GerenciadorDeOrdensDeServicoWeb.DataTransferObjects.tapetes;
using GerenciadorDeOrdensDeServicoWeb.DataTransferObjects.clientes;

namespace GerenciadorDeOrdensDeServicoWeb.DataTransferObjects.servicos {
	public class ValorDeServico {

		private UInt32 _codigo;
		private UInt32 _codigoPai;
		private UInt32 _codigoServico;
		private Tapete _tapete;
		private TipoDeCliente _tipoDeCliente;
		private double _valorInicial;
		private double _valorAcima10m2;
		
		private List<ValorDeServico> _valoresEspeciais;

		public ValorDeServico() {
			_codigo = 0;
			_codigoPai = 0;
			_codigoServico = 0;
			_tapete = new Tapete();
			_tipoDeCliente = new TipoDeCliente();
			_valorInicial = 0;
			_valorAcima10m2 = 0;
			
			_valoresEspeciais = new List<ValorDeServico>();
		}

		public UInt32 codigo {
			get { return _codigo; }
			set { _codigo = value; }
		}

		public UInt32 codigoPai {
			get { return _codigoPai; }
			set { _codigoPai = value; }
		}

		public UInt32 codigoServico {
			get { return _codigoServico; }
			set { _codigoServico = value; }
		}

		public Tapete tapete {
			get { return _tapete; }
			set { _tapete = value; }
		}

		public TipoDeCliente tipoDeCliente {
			get { return _tipoDeCliente; }
			set { _tipoDeCliente = value; }
		}

		public double valorInicial {
			get { return _valorInicial; }
			set { _valorInicial = value; }
		}

		public double valorAcima10m2 {
			get { return _valorAcima10m2; }
			set { _valorAcima10m2 = value; }
		}

		public List<ValorDeServico> valoresEspeciais {
			get { return _valoresEspeciais; }
			set { _valoresEspeciais = value; }
		}
	}
}