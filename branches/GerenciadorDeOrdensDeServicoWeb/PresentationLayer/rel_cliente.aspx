  <%@ Page Language="C#" %>
        <!-- Importando NameSpaces de Dados -->
        <%@ Import Namespace="System.Data" %>
        <%@ Import Namespace="MySql.Data.MySqlClient" %>
        <%@ Import Namespace="GerenciadorDeOrdensDeServicoWeb.DataAccessLayer.DatabaseConnections" %>
        <%@ Import Namespace="MySql.Data" %>
        <%@ Import Namespace="System.Collections.Generic" %>
        <%@ Import Namespace =" MySql.Data.MySqlClient" %>


        
    <!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
    
    
    
    
    <script runat="server">

        
        protected void Page_Load(object sender, EventArgs e)
        {
           
        }
                  
        
        protected void Button1_Click(object sender, EventArgs e)
        {

            List<String> colunaSql = new List<String>();
            string str = "";





            bool Tipo = false;
            bool Conjuge = false;
            bool Cliente = false;
            bool Rg = false;
            bool Cpf = false;
            bool Contato = false;
            bool Modo = false;
            bool Detalhes = false;
            bool todos = false;


            if (chk_tipo.Checked == true)
                Tipo = true;
            if (chk_rg.Checked == true)
                Rg = true;
            if (chk_cpf.Checked == true)
                Cpf = true;
            if (chk_nome.Checked == true)
                Cliente = true;
            if (chk_conjuge.Checked == true)
                Conjuge = true;
            if (chk_contato.Checked == true)
                Contato = true;
            if (chk_tipo_contato.Checked == true)
                Modo = true;
            if (chk_detalhes.Checked == true)
                Detalhes = true;

            if (chk_todos.Checked == true)
            {
                todos = true;
                Tipo = true;
                Rg = true;
                Cpf = true;
                Cliente = true;
                Conjuge = true;
                Contato = true;
                Modo = true;
                Detalhes = true;
                
            }

            if (chk_todos.Checked == false)
            {
                todos = false;
               
            }


            
           
            try
            {

                //for (int i = 0; i < this.Controls.Count; i++)
                //{

                //    if (this.Controls[i] is System.Web.UI.WebControls.CheckBox)
                //    {

                //        if ((this.Controls[i] as CheckBox).Checked = false)
                //            lbl_rel.Text = " Nao a colunas para consumir!";
                //        return;
                //    }
                //}


                if (Cliente)
                {
                    colunaSql.Add("(nom_cliente) as Cliente");
                }
                if (Tipo)
                {
                    colunaSql.Add("CASE cod_tipo_cliente WHEN 1 THEN 'Cliente Comun' WHEN 2 THEN 'Lavanderia Comun' WHEN 3 THEN 'Decoradora' end as Tipo");
                }
                if (Rg)
                {
                    colunaSql.Add("(txt_rg) as Rg");
                }
                if (Cpf)
                {
                    colunaSql.Add("(txt_cpf) as Cpf");
                }
                if (Contato)
                {
                    colunaSql.Add("(tb_meios_de_contato.txt_meio_de_contato) as Contato");
                }

                if (Modo)
                {
                    colunaSql.Add("CASE tb_meios_de_contato.cod_tipo_meio_de_contato WHEN 1 THEN 'Tel.Residencial' WHEN 2 THEN 'Tel Comercial' WHEN 3 THEN 'Celular' WHEN 4 THEN 'Radio' WHEN 5 THEN 'Outros'  end as Modo");
                }
                if (Detalhes)
                {
                    colunaSql.Add("(tb_meios_de_contato.txt_descricao) as Detalhes");
                }
                if (Conjuge)
                {
                    colunaSql.Add("(nom_conjuge) as Conjuge");
                }

                string sql2 = "";

                sql2 += "SELECT {0} from tb_clientes inner join tb_meios_de_contato on tb_meios_de_contato.cod_cliente";
                sql2 += " = tb_clientes.cod_cliente";
                sql2 += " where ";
                sql2 += " (nom_cliente like '" + fl_nome.Text.ToString() + "' or  nom_cliente is null) ";
                sql2 += " or (txt_cpf like '" + fl_cpf.Text.ToString() + "' or  txt_cpf is null) ";
                sql2 += " or (txt_rg like '" + fl_rg.Text.ToString() + "' or  txt_rg is null) ";
                sql2 += " or (nom_conjuge like '" + fl_conjuge.Text.ToString() + "' or  txt_rg is null) ";
                sql2 += " or (cod_tipo_cliente  = " + fl_tipo.SelectedValue + " or  cod_tipo_cliente = 0) ";


                String sql = String.Format(sql2, String.Join(",", colunaSql.ToArray()));
                
                //("SELECT {0} from tb_clientes inner join tb_meios_de_contato on tb_meios_de_contato.cod_cliente = tb_clientes.cod_cliente",
                // String.Join(",", colunaSql.ToArray()));


                MySql.Data.MySqlClient.MySqlConnection connection = GerenciadorDeOrdensDeServicoWeb.DataAccessLayer.DatabaseConnections.MySqlConnectionWizard.getConnection();



                System.Data.DataSet ds = new System.Data.DataSet();
                MySql.Data.MySqlClient.MySqlDataAdapter adp = new MySql.Data.MySqlClient.MySqlDataAdapter();

                adp.SelectCommand = new MySql.Data.MySqlClient.MySqlCommand(sql, connection);
                adp.Fill(ds);

                System.Data.DataTable table = ds.Tables[0];



                str += "<table width=100% border=0 cellspacing =1 >";


                // escreve o NOME das colunas de acordo com os ALIAS DO SELECT
                Response.Write("<tr style=background-color:Silver>");
                foreach (System.Data.DataColumn col in table.Columns)
                {
                    str += "<td align=left  style=font-weight:bold;background-color:Silver>" + col.ToString() + "</td>";
                }
                Response.Write("</tr>");


                string cor = "ActiveCaption";
                foreach (System.Data.DataRow row in table.Rows)
                {
                    str += "<tr style=background-color:"+cor+" >";
                    
                    for (int i = 0; i < row.ItemArray.Length; i++)
                    {
                        str += "<td align=left>" + row[i] + "</td>";
                    }
                    str += "</tr>";


                    if (cor == "ActiveCaption")
                    {
                        cor = "Green";
                    }
                    else
                    {
                        cor = "ActiveCaption";
                    }
                    
                }


                str += "</table>";


                lbl_rel.Text = str.ToString();
            }

            catch (Exception ex)
            {
                lbl_rel.Text = " Nao a colunas selecionadas!!";
            }
            
        }

        
        
        
        //Export excel//
        
        
        protected void Button2_Click(object sender, EventArgs e)
        {
            string str2 = "";


            #region oldcode

              List<String> colunaSql = new List<String>();
           





            bool Tipo = false;
            bool Conjuge = false;
            bool Cliente = false;
            bool Rg = false;
            bool Cpf = false;
            bool Contato = false;
            bool Modo = false;
            bool Detalhes = false;


            if (chk_tipo.Checked == true)
                Tipo = true;
            if (chk_rg.Checked == true)
                Rg = true;
            if (chk_cpf.Checked == true)
                Cpf = true;
            if (chk_nome.Checked == true)
                Cliente = true;
            if (chk_conjuge.Checked == true)
                Conjuge = true;
            if (chk_contato.Checked == true)
                Contato = true;
            if (chk_tipo_contato.Checked == true)
                Modo = true;
            if (chk_detalhes.Checked == true)
                Detalhes = true;

                         //for (int i = 0; i < this.Controls.Count; i++)
                //{

                //    if (this.Controls[i] is System.Web.UI.WebControls.CheckBox)
                //    {

                //        if ((this.Controls[i] as CheckBox).Checked = false)
                //            lbl_rel.Text = " Nao a colunas para consumir!";
                //        return;
                //    }
                //}


            try
            {
                if (Cliente)
                {
                    colunaSql.Add("(nom_cliente) as Cliente");
                }
                if (Tipo)
                {
                    colunaSql.Add("CASE cod_tipo_cliente WHEN 1 THEN 'Cliente Comun' WHEN 2 THEN 'Lavanderia Comun' WHEN 3 THEN 'Decoradora' end as Tipo");
                }
                if (Rg)
                {
                    colunaSql.Add("(txt_rg) as Rg");
                }
                if (Cpf)
                {
                    colunaSql.Add("(txt_cpf) as Cpf");
                }
                if (Contato)
                {
                    colunaSql.Add("(tb_meios_de_contato.txt_meio_de_contato) as Contato");
                }

                if (Modo)
                {
                    colunaSql.Add("CASE tb_meios_de_contato.cod_tipo_meio_de_contato WHEN 1 THEN 'Tel.Residencial' WHEN 2 THEN 'Tel Comercial' WHEN 3 THEN 'Celular' WHEN 4 THEN 'Radio' WHEN 5 THEN 'Outros'  end as Modo");
                }
                if (Detalhes)
                {
                    colunaSql.Add("(tb_meios_de_contato.txt_descricao) as Detalhes");
                }
                if (Conjuge)
                {
                    colunaSql.Add("(nom_conjuge) as Conjuge");
                }

                string sql2 = "";

                sql2 += "SELECT {0} from tb_clientes inner join tb_meios_de_contato on tb_meios_de_contato.cod_cliente";
                sql2 += " = tb_clientes.cod_cliente";
                sql2 += " where ";
                sql2 += " (nom_cliente like '" + fl_nome.Text.ToString() + "' or  nom_cliente is null) ";
                sql2 += " or (txt_cpf like '" + fl_cpf.Text.ToString() + "' or  txt_cpf is null) ";
                sql2 += " or (txt_rg like '" + fl_rg.Text.ToString() + "' or  txt_rg is null) ";
                sql2 += " or (nom_conjuge like '" + fl_conjuge.Text.ToString() + "' or  txt_rg is null) ";
                sql2 += " or (cod_tipo_cliente  = " + fl_tipo.SelectedValue + " or  cod_tipo_cliente = 0) ";


                String sql = String.Format(sql2, String.Join(",", colunaSql.ToArray()));
                
                //("SELECT {0} from tb_clientes inner join tb_meios_de_contato on tb_meios_de_contato.cod_cliente = tb_clientes.cod_cliente",
                // String.Join(",", colunaSql.ToArray()));


                MySql.Data.MySqlClient.MySqlConnection connection = GerenciadorDeOrdensDeServicoWeb.DataAccessLayer.DatabaseConnections.MySqlConnectionWizard.getConnection();



                System.Data.DataSet ds = new System.Data.DataSet();
                MySql.Data.MySqlClient.MySqlDataAdapter adp = new MySql.Data.MySqlClient.MySqlDataAdapter();

                adp.SelectCommand = new MySql.Data.MySqlClient.MySqlCommand(sql, connection);
                adp.Fill(ds);

                System.Data.DataTable table = ds.Tables[0];

            
            

#endregion
                
            

            str2 += "<table width=100% border=0 cellspacing =1 >";


            // escreve o NOME das colunas de acordo com os ALIAS DO SELECT
           str2 += "<tr style=background-color:Silver>";
            foreach (System.Data.DataColumn col in table.Columns)
            {
                str2 += "<td align=left  style=font-weight:bold;background-color:Silver>" + col.ToString() + "</td>";
            }
            str2 += "</tr>";


            string cor = "ActiveCaption";
            foreach (System.Data.DataRow row in table.Rows)
            {
                str2 += "<tr style=background-color:" + cor + " >";

                for (int i = 0; i < row.ItemArray.Length; i++)
                {
                    str2 += "<td align=left>" + row[i] + "</td>";
                }
                str2 += "</tr>";


                if (cor == "ActiveCaption")
                {
                    cor = "Green";
                }
                else
                {
                    cor = "ActiveCaption";
                }

            }


            str2 += "</table>";

           

                Response.Clear();
                Response.Buffer = true;
                string filename = "Relatorio_" + String.Format("{0:dd_MMM_yy}", DateTime.Now.Date) + "Clientes.xls";
                Response.ContentType = "application/vnd.ms-excel";
                Response.AddHeader("Content-Disposition", "attachment;filename=" + filename);
                String str = str2.ToString();
                Response.Write(str);
                Response.End();

            
            }
            catch (Exception ex) {
                lbl_rel.Text = "Sem dados selecionados!!.";
            
            }

            
           
        }
        
        
        
</script>


    <html xmlns="http://www.w3.org/1999/xhtml">
        <head id="Head1" runat="server">
                <title> Relatório</title>
     
               
        </head>
        <body style="background-color:White">
                <form id="form" runat="server">

              <div></div>
              

              <table align="center" style="background-color:Navy" width="65%" height=40px">
              <tr align="center" valign="top" style="color:White;font-size:small" >
                  

              <td align="left" style="background-color:Gray"><b>Filtros:</b></td>
              <td><b>Nome</b>   <br /> <asp:TextBox ID="fl_nome" AutoPostBack="false"  runat="server"></asp:TextBox>     </td>
              <td><b>Conjuge</b> <br /> <asp:TextBox ID="fl_conjuge" runat="server"></asp:TextBox>  </td>

             

              <td><b>Tipo</b> <br /> 
              
              
                <asp:DropDownList ID="fl_tipo" runat="server">
                <asp:ListItem Enabled ="true" Text =" Cliente Comun " Value="1"></asp:ListItem>
                <asp:ListItem Enabled ="true" Text =" Lavanderia " Value="2"></asp:ListItem>
                <asp:ListItem Enabled ="true" Text =" Decoradora " Value="3"></asp:ListItem>
                <asp:ListItem Enabled ="true" Text =" Todos " Value="0"></asp:ListItem>
                
                 </asp:DropDownList> 
              
              
              
               </td>



              <td><b>RG</b>   <br />  <asp:TextBox ID="fl_rg" runat="server"></asp:TextBox>    </td>
              <td><b>CPF</b> <br />   <asp:TextBox ID="fl_cpf" runat="server"></asp:TextBox>   </td>
              
              </tr>
              </table>


             
              <table align="center" style="background-color:White" width="65%" height=50px">
              <tr valign="top" style="color:Black;font-size:small">
               
               <td style="background-color:White" width="130px"> <b>Dados:</b> </td>
               <td><b>Nome do Cliente</b> <br />   <asp:CheckBox ID="chk_nome" runat="server" />  </td>
               <td><b>Tipo de Cliente</b> <br /> <asp:CheckBox ID="chk_tipo" runat="server" />      </td>
               <td><b>Rg do Cliente</b>   <br /> <asp:CheckBox ID="chk_rg" runat="server" />    </td>
               <td><b>Cpf do Cliente</b> <br /> <asp:CheckBox ID="chk_cpf" runat="server" />  </td>
               <td><b>Dados de Contato</b> <br />   <asp:CheckBox ID="chk_contato" runat="server" />   </td>
               <td><b>Tipo de Contato</b> <br />   <asp:CheckBox ID="chk_tipo_contato" runat="server" />   </td>
               <td><b>Conjuge do Cliente</b>   <br />  <asp:CheckBox ID="chk_conjuge" runat="server" />    </td>
               <td><b>Detalhes</b> <br />   <asp:CheckBox ID="chk_detalhes" runat="server" />   </td>
               <td><b>Todos</b> <br />   <asp:CheckBox ID="chk_todos" runat="server" />   </td>
              
            
                  
              </tr>
              </table>
              <br />

              <div align = "center">

                <asp:Button ID="Button1" runat="server" Text="Gerar Relatório" onclick="Button1_Click" />
                  <asp:Button ID="Button2" runat="server" Text="Exportar para Excel" 
                      onclick="Button2_Click" />

                </div>

                 <h2 style="color:Black;font-family:Verdana">Relatório de Clientes</h2>

                  
                       
                        <br />

                <asp:Label ID="lbl_rel" runat="server" Text=""></asp:Label>
                    
                         



                </form>


        </body>
        </html>
