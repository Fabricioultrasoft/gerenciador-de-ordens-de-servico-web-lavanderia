using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GerenciadorDeOrdensDeServicoWeb.DataTransferObjects.tapetes;
using GerenciadorDeOrdensDeServicoWeb.DataTransferObjects.clientes;

namespace GerenciadorDeOrdensDeServicoWeb.DataTransferObjects.servicos {
	public class ValorDeServico {

		private UInt32 _codigo;
		private UInt32 _codigoServico;
		private double _valor;
		private double _valorAcima10m2;
		private Tapete _tapete;
		private List<ValorEspecial> _valoresEspeciais;

		public ValorDeServico() {
			_codigo = 0;
			_codigoServico = 0;
			_valor = 0;
			_valorAcima10m2 = 0;
			_tapete = new Tapete();
			_valoresEspeciais = new List<ValorEspecial>();
		}

		public UInt32 codigo {
			get { return _codigo; }
			set { _codigo = value; }
		}

		public UInt32 codigoServico {
			get { return _codigoServico; }
			set { _codigoServico = value; }
		}

		public double valor {
			get { return _valor; }
			set { _valor = value; }
		}

		public double valorAcima10m2 {
			get { return _valorAcima10m2; }
			set { _valorAcima10m2 = value; }
		}

		public Tapete tapete {
			get { return _tapete; }
			set { _tapete = value; }
		}

		public List<ValorEspecial> valoresEspeciais {
			get { return _valoresEspeciais; }
			set { _valoresEspeciais = value; }
		}
	}
}