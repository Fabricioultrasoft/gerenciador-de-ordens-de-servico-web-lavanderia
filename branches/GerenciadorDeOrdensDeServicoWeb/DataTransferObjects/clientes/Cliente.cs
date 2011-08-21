using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GerenciadorDeOrdensDeServicoWeb.DataTransferObjects.enderecos;
using GerenciadorDeOrdensDeServicoWeb.DataTransferObjects.usuarios;

namespace GerenciadorDeOrdensDeServicoWeb.DataTransferObjects.clientes {
	
	public enum Sexo : int {
		masculino = 1,
		feminino = 2
	}
	
	public class Cliente {

		private UInt32 _codigo;
		private String _nome;
		private String _conjuge;
		private TipoDeCliente _tipoDeCliente;
		private Sexo _sexo;
		private DateTime? _dataDeNascimento;// com o caracter especial "?" depois do tipo do atributo, é possivel dizer que este aceita null
		private String _rg;
		private String _cpf;
		private String _observacoes;
		private bool _ativo;
		private DateTime _dataDeCadastro;
		private DateTime _dataDeAtualizacao;
		private List<MeioDeContato> _meiosDeContato;
		private List<Endereco> _enderecos;
		private Usuario _usuario;

		public Cliente() {
			this.codigo = 0;
			this.nome = String.Empty;
			this.conjuge = String.Empty;
			this.tipoDeCliente = new TipoDeCliente();
			this.sexo = Sexo.masculino;
			this.dataDeNascimento = null;
			this.rg = String.Empty;
			this.cpf = String.Empty;
			this.observacoes = String.Empty;
			this.ativo = true;
			this.dataDeCadastro = DateTime.Now;
			this.dataDeAtualizacao = DateTime.Now;
			this.meiosDeContato = new List<MeioDeContato>();
			this.enderecos = new List<Endereco>();
			this.usuario = new Usuario();
		}

		public Cliente(UInt32 codigo,String nome,String conjuge,TipoDeCliente tipoDeCliente, Sexo sexo, DateTime? dataDeNascimento,
				String rg, String cpf, String observacoes, bool ativo, DateTime dataDeCadastro, DateTime dataDeAtualizacao,
				List<MeioDeContato> meiosDeContato, List<Endereco> enderecos, Usuario usuario) {
			this.codigo = codigo;
			this.nome = nome;
			this.conjuge = conjuge;
			this.tipoDeCliente = tipoDeCliente;
			this.sexo = sexo;
			this.dataDeNascimento = dataDeNascimento;
			this.rg = rg;
			this.cpf = cpf;
			this.observacoes = observacoes;
			this.ativo = ativo;
			this.dataDeCadastro = dataDeCadastro;
			this.dataDeAtualizacao = dataDeAtualizacao;
			this.meiosDeContato = meiosDeContato;
			this.enderecos = enderecos;
			this.usuario = usuario;
		}

		public UInt32 codigo {
			get { return _codigo; }
			set { _codigo = value; }
		}
		public String nome {
			get { return _nome; }
			set { _nome = value; }
		}
		public String conjuge {
			get { return _conjuge; }
			set { _conjuge = value; }
		}
		public TipoDeCliente tipoDeCliente {
			get { return _tipoDeCliente; }
			set { _tipoDeCliente = value; }
		}
		public Sexo sexo {
			get { return _sexo; }
			set { _sexo = value; }
		}
		public DateTime? dataDeNascimento {
			get { return _dataDeNascimento; }
			set { _dataDeNascimento = value; }
		}
		public String rg {
			get { return _rg; }
			set { _rg = value; }
		}
		public String cpf {
			get { return _cpf; }
			set { _cpf = value; }
		}
		public String observacoes {
			get { return _observacoes; }
			set { _observacoes = value; }
		}
		public bool ativo {
			get { return _ativo; }
			set { _ativo = value; }
		}
		public DateTime dataDeCadastro {
			get { return _dataDeCadastro; }
			set { _dataDeCadastro = value; }
		}
		public DateTime dataDeAtualizacao {
			get { return _dataDeAtualizacao; }
			set { _dataDeAtualizacao = value; }
		}
		public List<MeioDeContato> meiosDeContato {
			get { return _meiosDeContato; }
			set { _meiosDeContato = value; }
		}
		public List<Endereco> enderecos {
			get { return _enderecos; }
			set { _enderecos = value; }
		}
		public Usuario usuario {
			get { return _usuario; }
			set { _usuario = value; }
		}
	}
}