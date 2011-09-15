using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GerenciadorDeOrdensDeServicoWeb.DataTransferObjects.clientes;
using GerenciadorDeOrdensDeServicoWeb.DataTransferObjects.usuarios;

namespace GerenciadorDeOrdensDeServicoWeb.DataTransferObjects.ordensDeServico {
	public class OrdemDeServico {

		private UInt32 _codigo;
		private UInt32 _numero;
		private Cliente _cliente;
		private Usuario _usuario; // usuario que cadastrou a OS
		private Status _status;
		private List<Item> _itens;
		private String _observacoes;

		private double _valorOriginal; // valor calculado com base nos valores dos serviços para cada tapete de acordo com sua metragem
		private double _valorFinal; // valor da OS caso o usuario altere o valor original

		private DateTime _dataDeAbertura; // usuario deve informar a data de abertura da OS
		private DateTime _previsaoDeConclusao; // usuario deve informar a previsao para a OS ser concluida e entregue
		private DateTime _dataDeEncerramento; // usuario deve informar a data que a OS foi finalizada


		/*+---------------------------------+
		  | reservado para o banco de dados |
		  +---------------------------------+*/
		private DateTime _dataDeCadastro; // data que o registro foi inserido no banco
		private DateTime _dataDeAtualizacao; // data da ultima atualizacao do registro

		public OrdemDeServico() {
			DateTime datAux = DateTime.Now;

			_codigo = 0;
			_numero = 0;
			_cliente = new Cliente();
			_usuario = new Usuario();
			_status = new Status();
			_itens = new List<Item>();
			_observacoes = String.Empty;
			_valorOriginal = 0;
			_valorFinal = 0;
			_dataDeAbertura = datAux;
			_previsaoDeConclusao = datAux;
			_dataDeEncerramento = datAux;
		}

		public OrdemDeServico( UInt32 codigo, UInt32 numero, Cliente cliente, Usuario usuario, Status status, String observacoes,
			double valorOriginal, double valorFinal, DateTime dataDeAbertura, DateTime previsaoDeConclusao, DateTime dataDeEncerramento ) {

			_codigo = codigo;
			_numero = numero;
			_cliente = cliente;
			_usuario = usuario;
			_status = status;
			_itens = new List<Item>();
			_observacoes = observacoes;
			_valorOriginal = valorOriginal;
			_valorFinal = valorFinal;
			_dataDeAbertura = dataDeAbertura;
			_previsaoDeConclusao = previsaoDeConclusao;
			_dataDeEncerramento = dataDeEncerramento;
		}

		public UInt32 codigo {
			get { return _codigo; }
			set { _codigo = value; }
		}
		public UInt32 numero {
			get { return _numero; }
			set { _numero = value; }
		}
		public Cliente cliente {
			get { return _cliente; }
			set { _cliente = value; }
		}
		public Usuario usuario {
			get { return _usuario; }
			set { _usuario = value; }
		}
		public Status status {
			get { return _status; }
			set { _status = value; }
		}
		public List<Item> itens {
			get { return _itens; }
			set { _itens = value; }
		}
		public String observacoes {
			get { return _observacoes; }
			set { _observacoes = value; }
		}
		public double valorOriginal {
			get { return _valorOriginal; }
			set { _valorOriginal = value; }
		}
		public double valorFinal {
			get { return _valorFinal; }
			set { _valorFinal = value; }
		}
		public DateTime dataDeAbertura {
			get { return _dataDeAbertura; }
			set { _dataDeAbertura = value; }
		}
		public DateTime dataDeEncerramento {
			get { return _dataDeEncerramento; }
			set { _dataDeEncerramento = value; }
		}
		public DateTime previsaoDeConclusao {
			get { return _previsaoDeConclusao; }
			set { _previsaoDeConclusao = value; }
		}
		public DateTime dataDeCadastro {
			get { return _dataDeCadastro; }
			set { _dataDeCadastro = value; }
		}
		public DateTime dataDeAtualizacao {
			get { return _dataDeAtualizacao; }
			set { _dataDeAtualizacao = value; }
		}
	}
}