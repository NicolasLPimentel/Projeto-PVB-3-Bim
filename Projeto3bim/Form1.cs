/* *******************************************************************
 * Colegio Técnico Antônio Teixeira Fernandes (Univap)
 * Curso Técnico em Informática - Data de Entrega: 10/09/2024
 * Autores do Projeto: Nicolas L. Pimentel
 *                     
 * Turma: 2H
 * Atividade Proposta em aula
 * Observação:
 * 
 * BUTTON:
 *  button1 - botão para enviar os dados da compra
 *  button2 - botão para pagar parcela
 *  
 * ************************************************************
 *  
 * LABEL:
 *  label1 - "Insira o valor da compra:"
 *  label2 - "Insira a data da compra:"
 *  label3 - "(dd/MM/yyyy)"
 *  label4 - "Insira a quantia de parcelas:"
 *  label5 - "Total a pagar:"
 *  label6 - recebe o valor total que ainda resta pagar
 *  label7 - "Insira a data do pagamento da parcela:"
 *  label8 - "(dd/MM/yyyy)"
 *  label9 - recebe o valor da parcela ajustado com juros
 *  
 * ************************************************************
 *
 * TEXTBOX:
 *  textBox1 - caixa de texto que recebe o valor da compra
 *  textBox2 - caixa de texto que recebe a data da compra
 *  textBox3 - caixa de texto que recebe a quantia de parcelas
 *  textBox4 - caixa de texto multilinha que recebe os vencimentos das parcelas
 *  textBox5 - caixa de texto que recebe a data do pagamento da parcela
 *  textBox6 - caixa de texto multilinha que recebe os vencimentos das parcelas atualizados
 *  
 * problema_aula.cs
 * ************************************************************/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Projeto3bim
{
    public partial class Form1 : Form
    {
        int cont_linha = 5;
        double aux_valor_compra; //double pois realiza um calculo com um valor double
        int cont_pago = 0;

        public Form1()
        {
            InitializeComponent();
            label9.Text = "";
            textBox6.Visible = false;
            textBox4.Text = "----------------PARCELAS----------------" + Environment.NewLine + Environment.NewLine;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox4.Text = "----------------PARCELAS----------------" + Environment.NewLine + Environment.NewLine;
            textBox6.Visible = false;
            textBox4.Visible = true;
            cont_pago = 0;
            cont_linha = 5;
            double valor_compra = double.Parse(textBox1.Text);
            aux_valor_compra = valor_compra;
            label6.Text = (String.Format("{0:C2}", valor_compra));
            int quantia_parcela = int.Parse(textBox3.Text);
            double valor_parcela = valor_compra / quantia_parcela;
            DateTime.TryParse(textBox2.Text, out DateTime data_inicial);
            data_inicial = data_inicial.Date;

            for (int x = 1; x <= quantia_parcela; x++)
            {
                DateTime data_vencimento = data_inicial.AddMonths(x);

                while (data_vencimento.ToString("ddd") == "sáb" || data_vencimento.ToString("ddd") == "dom")
                    data_vencimento = data_vencimento.AddDays(1);

                textBox4.AppendText(String.Format("Preço da {0}ª parcela: {1:C2}", x, valor_parcela) + Environment.NewLine + "Data de vencimento: " + data_vencimento.ToString("dd/MM/yyyy"));
                textBox4.AppendText(" -> " + data_vencimento.ToString("ddd") + Environment.NewLine + Environment.NewLine);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            label9.Text = "";
            DateTime.TryParse(textBox2.Text, out DateTime data_inicial);
            DateTime.TryParse(textBox5.Text, out DateTime data_inserida);
            data_inicial = data_inicial.Date;
            data_inserida = data_inserida.Date;
            double valor_parcela_atrasada = 0;
            int quantia_parcela = int.Parse(textBox3.Text);
            double valor_compra = double.Parse(textBox1.Text);
            double valor_parcela = valor_compra / quantia_parcela;

            if (cont_pago < quantia_parcela) //teste feito para nao alterar o valor das variaveis mesmo quando as parcelas acabarem
            {
                if (data_inserida < data_inicial)
                {
                    label9.Text = "AVISO: A DATA INSERIDA É INVÁLIDA";
                }
                else
                {
                    textBox4.Visible = false;
                    textBox6.Visible = true;

                    DateTime data_vencimento = data_inicial.AddMonths(1 + cont_pago);

                    while (data_vencimento.ToString("ddd") == "sáb" || data_vencimento.ToString("ddd") == "dom")
                        data_vencimento = data_vencimento.AddDays(1);

                    valor_parcela_atrasada = valor_parcela;

                    while (data_inserida > data_vencimento)
                    {
                        valor_parcela_atrasada += valor_parcela_atrasada * 0.03;

                        data_vencimento = data_vencimento.AddMonths(1);

                        while (data_vencimento.ToString("ddd") == "sáb" || data_vencimento.ToString("ddd") == "dom")
                            data_vencimento = data_vencimento.AddDays(1);
                    }

                    if (valor_parcela_atrasada > valor_parcela) //se houve juros aplicado o label9 é alterado
                    {
                        MessageBox.Show("AVISO: O PAGAMENTO DA PARCELA ESTÁ ATRASADO:");
                        label9.Text += String.Format("PARCELA REAJUSTADA COM JUROS: {0:C2}", valor_parcela_atrasada);
                    }

                    if (aux_valor_compra > 0)
                        label6.Text = (String.Format("{0:C2}", aux_valor_compra -= valor_parcela));

                    textBox6.Text = "----------------PARCELAS----------------" + Environment.NewLine + Environment.NewLine;

                    for (int x = cont_linha; x < textBox4.Lines.Length; x++)
                        textBox6.AppendText(textBox4.Lines[x] + Environment.NewLine);

                    cont_linha += 3;
                    cont_pago++;
                }
            }
        }
    }
}
