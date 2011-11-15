using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;

namespace GerenciadorDeOrdensDeServicoWeb.DataTransferObjects {
	public class Erro {

		private int _numeroDoErro;
		private String _mensagem;
		private String _solucao;

		public Erro(){
			numeroDoErro =  0;
			_mensagem = String.Empty;
			_solucao = String.Empty;
		}

		public Erro( int number ) {
			numeroDoErro = number;

			switch( number ) {
				case (int) MySqlErrorCode.UnableToConnectToHost: //1042
					_mensagem = "Não foi possivel estabelecer uma conexão com o banco de dados";
					_solucao = "Verifique se o banco de dados encontra-se em execução";
					break;

				case 0:
					_mensagem = String.Empty;
					_solucao = "Contate o Fornecedor";
					break;

				default:
					_mensagem = "Não definido";
					_solucao = "Não definido";
					break;
			}
		}

		public Erro( int number, String message ) {
			numeroDoErro = number;
			_mensagem = message;
			_solucao = "Contate o Fornecedor";
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