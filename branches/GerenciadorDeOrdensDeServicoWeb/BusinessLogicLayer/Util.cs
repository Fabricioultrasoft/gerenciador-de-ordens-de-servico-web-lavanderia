using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;

namespace GerenciadorDeOrdensDeServicoWeb.BusinessLogicLayer {
	public class Util {
		/// <summary>
		/// valida data no formato [dd-MM-yyyy HH:mm:ss]
		/// </summary>
		/// <param name="strDateTime">String contendo a data a ser validada</param>
		/// <returns>true se strDateTime for uma data valida no formato [dd-MM-yyyy HH:mm:ss]</returns>
		public static bool isDateTime( string strDateTime ) {
			Regex date = new Regex( @"^((((0?[1-9]|[12]\d|3[01])[\-](0?[13578]|1[02])[\-]((1[6-9]|[2-9]\d)?\d{2}))|((0?[1-9]|[12]\d|30)[\-](0?[13456789]|1[012])[\-]((1[6-9]|[2-9]\d)?\d{2}))|((0?[1-9]|1\d|2[0-8])[\-]0?2[\-]((1[6-9]|[2-9]\d)?\d{2}))|(29[\-]0?2[\-]((1[6-9]|[2-9]\d)?(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)|00)))|(((0[1-9]|[12]\d|3[01])(0[13578]|1[02])((1[6-9]|[2-9]\d)?\d{2}))|((0[1-9]|[12]\d|30)(0[13456789]|1[012])((1[6-9]|[2-9]\d)?\d{2}))|((0[1-9]|1\d|2[0-8])02((1[6-9]|[2-9]\d)?\d{2}))|(2902((1[6-9]|[2-9]\d)?(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)|00))))$" );
			return date.IsMatch( strDateTime );
		}

		/// <summary>
		/// valida CEP no formato [00000-000]
		/// </summary>
		/// <param name="strCEP">String contendo o CEP a ser validado</param>
		/// <returns>true se strCEP for um CEP valido no formato [00000-000]</returns>
		public static bool isCEP( String strCEP ) {
			Regex cep = new Regex( @"^[0-9]{5}-[0-9]{3}$" );
			return cep.IsMatch( strCEP );
		}


		public static long getNumeros( String str ) {
			String strNumber = String.Empty;
			long resultNumber = 0;

			Regex regex = new Regex( @"\d+" );
			MatchCollection match = regex.Matches( str );
			if( match.Count > 0 ) {
				for( int c = 0; c < match.Count; c++ ) {
					strNumber += match[c].Value;
				}
			}

			long.TryParse( strNumber, out resultNumber );

			return resultNumber;
		}

		public static byte[] stringToBytes( string str ) {
			System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
			return encoding.GetBytes( str );
		}

		public static String bytesToString( byte [] bytes ) {
			System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
			return encoding.GetString( bytes );
		}

		public static string bytesToHex( byte[] bytes ) {
			string hex = BitConverter.ToString( bytes );
			return hex.Replace( "-", "" );
		}
	}
}