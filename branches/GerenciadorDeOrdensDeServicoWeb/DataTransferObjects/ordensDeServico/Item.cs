using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GerenciadorDeOrdensDeServicoWeb.DataTransferObjects.tapetes;

namespace GerenciadorDeOrdensDeServicoWeb.DataTransferObjects.ordensDeServico {
	public class Item {

		private UInt32 _codigo;
		private UInt32 _codigoOrdemDeServico;
		private Tapete _tapete;
		private List<ItemServico> _itensServicos;
		private float _comprimento;
		private float _largura;
		private double _area;
		private double _valor;
		private double _m_m2;
		private String _observacoes;

		public Item() {
			_codigo = 0;
			_codigoOrdemDeServico = 0;
			_tapete = new Tapete();
			_comprimento = 0;
			_largura = 0;
			_area = 0;
			_valor = 0;
			_m_m2 = 0;
			_itensServicos = new List<ItemServico>();
			_observacoes = String.Empty;
		}

		public Item( UInt32 codigo, UInt32 codigoOrdemDeServico, Tapete tapete, float comprimento, float largura, String observacoes ) {
			_codigo = codigo;
			_codigoOrdemDeServico = codigoOrdemDeServico;
			_tapete = tapete;
			_comprimento = comprimento;
			_largura = largura;
			_area = comprimento * largura;
			_valor = 0;
			_m_m2 = 0;
			_itensServicos = new List<ItemServico>();
			_observacoes = observacoes;
		}

		public UInt32 codigo {
			get { return _codigo; }
			set { _codigo = value; }
		}
		public UInt32 codigoOrdemDeServico {
			get { return _codigoOrdemDeServico; }
			set { _codigoOrdemDeServico = value; }
		}
		public Tapete tapete {
			get { return _tapete; }
			set { _tapete = value; }
		}
		public List<ItemServico> itensServicos {
			get { return _itensServicos; }
			set { _itensServicos = value; }
		}
		public float comprimento {
			get { return _comprimento; }
			set { 
				_comprimento = value;
				_area = _comprimento * _largura;
			}
		}
		public float largura {
			get { return _largura; }
			set { 
				_largura = value;
				_area = _comprimento * _largura;
			}
		}
		public double area {
			get { return _area; }
		}
		public double valor {
			get { return _valor; }
			set { _valor = value; }
		}
		public double m_m2 {
			get { return _m_m2; }
			set { _m_m2 = value; }
		}
		public String observacoes {
			get { return _observacoes; }
			set { _observacoes = value; }
		}
	}
}