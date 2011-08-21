using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GerenciadorDeOrdensDeServicoWeb.DataTransferObjects {
	public class Erro {

		private int _numeroDoErro;
		private String _mensagem;
		private String _solucao;

		public Erro(){
			numeroDoErro =  0;
			_mensagem = "Não definido";
			_solucao = "Não definido";
		}

		public Erro(int number, String message, String solution){
			numeroDoErro = number;
			_mensagem = message;
			_solucao = solution;
		}

		public int numeroDoErro {
			get { return _numeroDoErro; }
			set { _numeroDoErro = value; }
		}

		public String mensagem {
			get { return _mensagem; }
			set { _mensagem = value; }
		}
		
		public String solucao {
			get { return _solucao; }
			set { _solucao = value; }
		}
	}
}