using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GerenciadorDeOrdensDeServicoWeb.DataTransferObjects.clientes;

namespace GerenciadorDeOrdensDeServicoWeb.DataTransferObjects.servicos {
	public class ValorEspecial {

		private UInt32 _codigo;
		private UInt32 _codigoValorDeServico;
		private double _valor;
		private double _valorAcima10m2;
		private TipoDeCliente _tipoDeCliente;

		public ValorEspecial() {
			_codigo = 0;
			_codigoValorDeServico = 0;
			_valor = 0;
			_valorAcima10m2 = 0;
			_tipoDeCliente = new TipoDeCliente();
		}

		public UInt32 codigo {
			get { return _codigo; }
			set { _codigo = value; }
		}

		public UInt32 codigoValorDeServico {
			get { return _codigoValorDeServico; }
			set { _codigoValorDeServico = value; }
		}

		public double valor {
			get { return _valor; }
			set { _valor = value; }
		}

		public double valorAcima10m2 {
			get { return _valorAcima10m2; }
			set { _valorAcima10m2 = value; }
		}

		public TipoDeCliente tipoDeCliente {
			get { return _tipoDeCliente; }
			set { _tipoDeCliente = value; }
		}
	}
}