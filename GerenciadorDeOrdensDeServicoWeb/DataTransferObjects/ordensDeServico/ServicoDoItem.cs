﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GerenciadorDeOrdensDeServicoWeb.DataTransferObjects.servicos;

namespace GerenciadorDeOrdensDeServicoWeb.DataTransferObjects.ordensDeServico {
	public class ServicoDoItem {

		private UInt32 _codigo;
		private UInt32 _codigoItem;
		private Servico _servico;
		private double _quantidade_m_m2; // quantidade de metros ou metros quadrados em que será realizado o serviço no tapete
		private double _valor;

		public ServicoDoItem() {
			_codigo = 0;
			_codigoItem = 0;
			_servico = new Servico();
			_quantidade_m_m2 = 0;
			_valor = 0;
		}

		public ServicoDoItem(UInt32 codigo, UInt32 codigoItem, Servico servico, double quantidade_m_m2, double valor ) {
			_codigo = codigo;
			_codigoItem = codigoItem;
			_servico = servico;
			_quantidade_m_m2 = quantidade_m_m2;
			_valor = valor;
		}

		public UInt32 codigo {
			get { return _codigo; }
			set { _codigo = value; }
		}
		public UInt32 codigoItem {
			get { return _codigoItem; }
			set { _codigoItem = value; }
		}
		public Servico servico {
			get { return _servico; }
			set { _servico = value; }
		}
		public double quantidade_m_m2 {
			get { return _quantidade_m_m2; }
			set { _quantidade_m_m2 = value; }
		}
		public double valor {
			get { return _valor; }
			set { _valor = value; }
		}
	}
}