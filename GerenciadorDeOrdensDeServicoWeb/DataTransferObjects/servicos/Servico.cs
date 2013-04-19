using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GerenciadorDeOrdensDeServicoWeb.DataTransferObjects.servicos {

	public enum CobradoPor : int {
		metro = 1,
		metroQuadrado = 2
	}

	public class Servico {

		private UInt32 _codigo;
		private String _nome;
		private String _descricao;
		private CobradoPor _cobradoPor;
		private List<ValorDeServico> _valores;

		public Servico() {
			_codigo = 0;
			_nome = String.Empty;
			_descricao = String.Empty;
			_cobradoPor = CobradoPor.metro;
			_valores = new List<ValorDeServico>();
		}

		public Servico( UInt32 codigo ) {
			_codigo = codigo;
			_nome = String.Empty;
			_descricao = String.Empty;
			_cobradoPor = CobradoPor.metro;
			_valores = new List<ValorDeServico>();
		}

		public UInt32 codigo {
			get { return _codigo; }
			set { _codigo = value; }
		}

		public String nome {
			get { return _nome; }
			set { _nome = value; }
		}

		public String descricao {
			get { return _descricao; }
			set { _descricao = value; }
		}

		public CobradoPor cobradoPor {
			get { return _cobradoPor; }
			set { _cobradoPor = value; }
		}

		public List<ValorDeServico> valores {
			get { return _valores; }
			set { _valores = value; }
		}
	}
}