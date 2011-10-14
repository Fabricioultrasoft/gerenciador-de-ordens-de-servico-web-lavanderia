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





            bool numero = false;
            bool cliente = false;
            bool status = false;
            bool abertura = false;
            bool fechamento = false;
            bool Valor = false;
            bool Valor_Final = false;
            
            bool todos = false;


            if (chk_numero.Checked == true)
                numero = true;
            if (chk_cliente.Checked == true)
                cliente = true;
            if (chk_status.Checked == true)
                status = true;
            if (chk_abertura.Checked == true)
                abertura = true;
            if (chk_fechamento.Checked == true)
                fechamento = true;
            if (chk_valorori.Checked == true)
                Valor = true;
            if (chk_valorfinal.Checked == true)
                Valor_Final = true;

            if (chk_todos.Checked == true)
            {
                todos = true;
                numero = true;
                status = true;
                abertura = true;

                fechamento = true;
                 Valor = true;
                cliente = true;
               Valor_Final = true;
              
                
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


                if (numero)
                {
                    colunaSql.Add("(num_os) as Numero");
                }
                if (cliente)
                {
                    colunaSql.Add("(tb_clientes.nom_cliente) as Cliente");
                }
                if (status)
                {
                    colunaSql.Add("CASE cod_status_os WHEN 1 THEN 'Aberto' WHEN 2 THEN 'Finalizado' WHEN 3 THEN 'Cancelado' end as Status");
                }
                if (abertura)
                {
                    colunaSql.Add("(dat_abertura) as Abertura");
                }
                if (fechamento)
                {
                    colunaSql.Add("(dat_fechamento) as Fechamento");
                }
                if (Valor)
                {
                    colunaSql.Add("(val_original) as Valor");
                }
                if (Valor_Final)
                {
                    colunaSql.Add("(val_final) as Valor_Final");
                }
              

                string sql2 = "";

                sql2 += "SELECT {0} from tb_ordens_de_servico inner join tb_status_os on tb_status_os.cod_status_os = tb_ordens_de_servico.cod_status_os";
                sql2 += " inner join tb_clientes on tb_clientes.cod_cliente = tb_ordens_de_servico.cod_cliente";
              //  sql2 += " where ";
              //  sql2 += " (nom_cliente like '" + fl_nome.Text.ToString() + "' or  nom_cliente is null) ";
              //  sql2 += " or (txt_cpf like '" + fl_cpf.Text.ToString() + "' or  txt_cpf is null) ";
              //  sql2 += " or (txt_rg like '" + fl_rg.Text.ToString() + "' or  txt_rg is null) ";
              //  sql2 += " or (nom_conjuge like '" + fl_conjuge.Text.ToString() + "' or  txt_rg is null) ";
              //  sql2 += " or (cod_tipo_cliente  = " + fl_tipo.SelectedValue + " or  cod_tipo_cliente = 0) ";


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
              <td><b>N.Os</b>   <br /> <asp:TextBox ID="fl_nome" AutoPostBack="false"  runat="server"></asp:TextBox>     </td>
              <td><b>Cliente</b> <br /> <asp:TextBox ID="fl_conjuge" runat="server"></asp:TextBox>  </td>

             

              <td><b>Status</b> <br /> 
              
              
                <asp:DropDownList ID="fl_tipo" runat="server">
                <asp:ListItem Enabled ="true" Text ="Aberto " Value="1"></asp:ListItem>
                <asp:ListItem Enabled ="true" Text =" Finalizado " Value="2"></asp:ListItem>
                <asp:ListItem Enabled ="true" Text =" Cancelado " Value="3"></asp:ListItem>
                <asp:ListItem Enabled ="true" Text =" Todos " Value="0"></asp:ListItem>
                
                 </asp:DropDownList> 
              
              
              
               </td>



              <td><b>Abertura</b>   <br />  <asp:TextBox ID="fl_rg" runat="server"></asp:TextBox>    </td>
              <td><b>Fechamento</b> <br />   <asp:TextBox ID="fl_cpf" runat="server"></asp:TextBox>   </td>

              <td><b>Valor Original</b>   <br />  <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>    </td>
              <td><b>Valor Final</b> <br />       <asp:TextBox ID="TextBox2" runat="server"></asp:TextBox>   </td>
              
              </tr>
              </table>


             
              <table align="center" style="background-color:White" width="65%" height=50px">
              <tr valign="top" style="color:Black;font-size:small">
               
               <td style="background-color:White" width="130px"> <b>Dados:</b> </td>
               <td><b>N.OS</b> <br />   <asp:CheckBox ID="chk_numero" runat="server" />  </td>
               <td><b>Cliente</b> <br /> <asp:CheckBox ID="chk_cliente" runat="server" />      </td>
               <td><b>Status</b>   <br /> <asp:CheckBox ID="chk_status" runat="server" />    </td>
               <td><b>Abertura</b> <br /> <asp:CheckBox ID="chk_abertura" runat="server" />  </td>
               <td><b>Fechamento</b> <br />   <asp:CheckBox ID="chk_fechamento" runat="server" />   </td>
               <td><b>Valor Original</b> <br />   <asp:CheckBox ID="chk_valorori" runat="server" />   </td>
               <td><b>Valor Final</b>   <br />  <asp:CheckBox ID="chk_valorfinal" runat="server" />    </td>
              
               <td><b>Todos</b> <br />   <asp:CheckBox ID="chk_todos" runat="server" />   </td>
              
            
                  
              </tr>
              </table>
              <br />

              <div align = "center">

                <asp:Button ID="Button1" runat="server" Text="Gerar Relatório" onclick="Button1_Click" />
                  <asp:Button ID="Button2" runat="server" Text="Exportar para Excel" 
                       />

                </div>

                 <h2 style="color:Black;font-family:Verdana">Relatório de OS.</h2>

                  
                       
                        <br />

                <asp:Label ID="lbl_rel" runat="server" Text=""></asp:Label>
                    
                         



                </form>


        </body>
        </html>
