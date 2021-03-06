﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GerenciadorDeOrdensDeServicoWeb.DataTransferObjects;
using MySql.Data.MySqlClient;
using GerenciadorDeOrdensDeServicoWeb.DataTransferObjects.enderecos;
using GerenciadorDeOrdensDeServicoWeb.DataAccessLayer.DataAccessObjects.MySql.enderecos;

namespace GerenciadorDeOrdensDeServicoWeb.BusinessLogicLayer.enderecos {
	public class GerenciadorDeTiposDeLogradouros {

		public static List<Erro> preencher( out List<TipoDeLogradouro> tiposDeLogradouros, UInt32 start, UInt32 limit ) {
			List<Erro> erros = new List<Erro>();
			try {
				tiposDeLogradouros = MySqlTiposDeLogradourosDao.getTiposDeLogradouros( start, limit );
			} catch( MySqlException ex ) {
				// se houver um erro, preenche uma lista vazia
				tiposDeLogradouros = new List<TipoDeLogradouro>();

				if( ex.Number == (int) MySqlErrorCode.UnableToConnectToHost ) {
					erros.Add( new Erro( ex.Number ) );
				} else {
					erros.Add( new Erro( ex.Number, ex.Message ) );
				}
			}
			return erros;
		}

		public static List<Erro> cadastrar( ref List<TipoDeLogradouro> tiposDeLogradouros ) {
			List<Erro> erros = new List<Erro>();
			try {
				erros.AddRange( MySqlTiposDeLogradourosDao.inserir( ref tiposDeLogradouros ) );
			} catch( MySqlException ex ) {

				if( ex.Number == (int) MySqlErrorCode.UnableToConnectToHost ) {
					erros.Add( new Erro( ex.Number ) );
				} else {
					erros.Add( new Erro( ex.Number, ex.Message ) );
				}
			}
			return erros;
		}

		public static List<Erro> atualizar( List<TipoDeLogradouro> tiposDeLogradouros ) {
			List<Erro> erros = new List<Erro>();
			try {
				erros.AddRange( MySqlTiposDeLogradourosDao.atualizar( tiposDeLogradouros ) );
			} catch( MySqlException ex ) {

				if( ex.Number == (int) MySqlErrorCode.UnableToConnectToHost ) {
					erros.Add( new Erro( ex.Number ) );
				} else {
					erros.Add( new Erro( ex.Number, ex.Message ) );
				}
			}
			return erros;
		}

		public static List<Erro> excluir( List<TipoDeLogradouro> tiposDeLogradouros ) {
			List<Erro> erros = new List<Erro>();
			try {
				erros.AddRange( MySqlTiposDeLogradourosDao.excluir( tiposDeLogradouros ) );
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