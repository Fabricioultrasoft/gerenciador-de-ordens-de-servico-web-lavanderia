﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GerenciadorDeOrdensDeServicoWeb.DataTransferObjects;
using GerenciadorDeOrdensDeServicoWeb.DataTransferObjects.servicos;
using MySql.Data.MySqlClient;
using GerenciadorDeOrdensDeServicoWeb.DataAccessLayer.DataAccessObjects.MySql.servicos;
using GerenciadorDeOrdensDeServicoWeb.DataAccessLayer.DataAccessObjects.MySql.tapetes;
using GerenciadorDeOrdensDeServicoWeb.DataTransferObjects.tapetes;

namespace GerenciadorDeOrdensDeServicoWeb.BusinessLogicLayer.servicos {
	public class GerenciadorDeServicos {

		public static long count() {
			try {
				return MySqlServicosDao.countServicos();
			} catch {
				return 0;
			}
		}

		public static List<Erro> preencher( out List<Servico> servicos, UInt32 start, UInt32 limit ) {
			return preencher( out servicos, start, limit, false );
		}

		public static List<Erro> preencher( out List<Servico> servicos, UInt32 start, UInt32 limit, bool apenasDadosBasicos ) {
			List<Erro> erros = new List<Erro>();
			try {
				servicos = MySqlServicosDao.getServicos( start, limit, apenasDadosBasicos );
			} catch( MySqlException ex ) {
				// se houver um erro, preenche uma lista vazia
				servicos = new List<Servico>();

				if( ex.Number == (int) MySqlErrorCode.UnableToConnectToHost ) {
					erros.Add( new Erro( ex.Number ) );
				} else {
					erros.Add( new Erro( ex.Number, ex.Message ) );
				}
			}
			return erros;
		}

		public static List<Erro> preencher( UInt32 codigoTapete, UInt32 codigoTipoDeCliente, out List<Servico> servicos ) {
			List<Erro> erros = new List<Erro>();
			try {
				servicos = MySqlServicosDao.getServicosEspecificos( codigoTapete, codigoTipoDeCliente );
			} catch( MySqlException ex ) {
				// se houver um erro, preenche uma lista vazia
				servicos = new List<Servico>();

				if( ex.Number == (int) MySqlErrorCode.UnableToConnectToHost ) {
					erros.Add( new Erro( ex.Number ) );
				} else {
					erros.Add( new Erro( ex.Number, ex.Message ) );
				}
			}
			return erros;
		}

		public static List<Erro> preencher( UInt32 codigoServico, out Servico servico ) {
			List<Erro> erros = new List<Erro>();

			try {
				if( codigoServico == 0 ) {
					servico = new Servico();
					List<Tapete> tapetes = MySqlTapetesDao.getTapetes();
					foreach( Tapete tapete in tapetes ) {
						ValorDeServico val = new ValorDeServico();
						val.tapete.codigo = tapete.codigo;
						val.tapete.nome = tapete.nome;
						servico.valores.Add(val);
					}
				} else {
					servico = MySqlServicosDao.getServico( codigoServico );
				}
			} catch( MySqlException ex ) {

				servico = new Servico( codigoServico );

				if( ex.Number == (int) MySqlErrorCode.UnableToConnectToHost ) {
					erros.Add( new Erro( ex.Number ) );
				} else {
					erros.Add( new Erro( ex.Number, ex.Message ) );
				}
			}
			return erros;
		}

		public static List<Erro> cadastrar( ref List<Servico> servicos ) {
			List<Erro> erros = new List<Erro>();
			try {
				erros.AddRange( MySqlServicosDao.inserir( ref servicos ) );
			} catch( MySqlException ex ) {

				if( ex.Number == (int) MySqlErrorCode.UnableToConnectToHost ) {
					erros.Add( new Erro( ex.Number ) );
				} else {
					erros.Add( new Erro( ex.Number, ex.Message ) );
				}
			}
			return erros;
		}

		public static List<Erro> atualizar( ref List<Servico> servicos ) {
			List<Erro> erros = new List<Erro>();
			try {
				erros.AddRange( MySqlServicosDao.atualizar( ref servicos ) );
			} catch( MySqlException ex ) {

				if( ex.Number == (int) MySqlErrorCode.UnableToConnectToHost ) {
					erros.Add( new Erro( ex.Number ) );
				} else {
					erros.Add( new Erro( ex.Number, ex.Message ) );
				}
			}
			return erros;
		}

		public static List<Erro> excluir( List<Servico> servicos ) {
			List<Erro> erros = new List<Erro>();
			try {
				erros.AddRange( MySqlServicosDao.excluir( servicos ) );
			} catch( MySqlException ex ) {
				if( ex.Number == (int) MySqlErrorCode.UnableToConnectToHost ) {
					erros.Add( new Erro( ex.Number ) );
				} else {
					erros.Add( new Erro( ex.Number, ex.Message ) );
				}
			}
			return erros;
		}
	}
}