using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using GerenciadorDeOrdensDeServicoWeb.DataTransferObjects;
using System.Reflection;

namespace GerenciadorDeOrdensDeServicoWeb.PresentationLayer.app.handlers {
	public class Compartilhado {

		/// <summary>
		/// Metodo usado para construir a resposta em forma de json para o navegador,
		/// este constroi a parte da mensagem de erro para o usuario
		/// </summary>
		/// <param name="json"></param>
		/// <param name="erros"></param>
		public static void construirParteDoJsonMensagensDeErros( ref StringBuilder json, List<Erro> erros ) {
			json.AppendLine( " \"message\": [" );
			foreach( Erro err in erros ) {
				json.Append( " \" " );
				json.Append( "<p>" );
				json.Append( "<h6>Erro</h6>" );
				json.AppendFormat( "<b>Codigo:</b> {0}<br/>", err.numeroDoErro );
				json.AppendFormat( "<b>Causa:</b> {0}<br/>", err.mensagem.Replace( @"\", @"\\" ).Replace( "\n", "\\n" ).Replace( "\r", "\\r" ).Replace( "\"", "\\\"" ) );
				json.AppendFormat( "<b>Solução:</b> {0}", err.solucao.Replace( "\r\n", "" ) );
				json.Append( "</p>" );
				json.Append( " \"," );
			}
			if( erros.Count > 0 ) json.Remove( json.Length - 1, 1 );// remove a ultima virgula
			json.AppendLine( "]," );
		}

		public static T tratarCaracteresEspeciais<T>( T obj ) {
			Type type = obj.GetType();
			PropertyInfo[] properties = type.GetProperties();
			foreach( PropertyInfo property in properties ) {

				if( property.PropertyType == typeof( String ) ) {
					property.SetValue( obj,
						( (String) property.GetValue( obj,null ) )
							.Replace( @"\", @"\\" )
							.Replace( "\n", "\\n" )
							.Replace( "\r", "\\r" )
							.Replace( "\"", "\\\"" )
					,null);
				}
			}

			return obj;
		}
	}
}