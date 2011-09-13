﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GerenciadorDeOrdensDeServicoWeb.DataTransferObjects.tapetes;
using GerenciadorDeOrdensDeServicoWeb.DataTransferObjects.servicos;

namespace GerenciadorDeOrdensDeServicoWeb.DataTransferObjects.ordensDeServico {
	public class Item {

		private UInt32 _codigo;
		private UInt32 _codigoOrdemDeServico;
		private Tapete _tapete;
		private List<Servico> _servicos;
		private float _comprimento;
		private float _largura;
		private double _area;
		private double _valor;
		private String _observacoes;

		public Item() {
			_codigo = 0;
			_codigoOrdemDeServico = 0;
			_tapete = new Tapete();
			_comprimento = 0;
			_largura = 0;
			_area = 0;
			_valor = 0;
			_servicos = new List<Servico>();
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
			_servicos = new List<Servico>();
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
		public List<Servico> servicos {
			get { return _servicos; }
			set { _servicos = value; }
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
		public String observacoes {
			get { return _observacoes; }
			set { _observacoes = value; }
		}
	}
}