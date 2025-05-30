using CadParcial2Kecm;
using ClnParcial2Kecm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CpParcial2Kecm
{
    public partial class FrmSerie : Form
    {
        private bool esNuevo = false;
        public FrmSerie()
        {
            InitializeComponent();
        }

        private void listar()
        {
            var lista = SerieCln.listarPa(txtParametro.Text.Trim());
            dgvLista.DataSource = lista;
            dgvLista.Columns["id"].Visible = false;
            dgvLista.Columns["estado"].Visible = false;
            dgvLista.Columns["titulo"].HeaderText = "Tìtulo";
            dgvLista.Columns["sinopsis"].HeaderText = "Sinopsis";
            dgvLista.Columns["director"].HeaderText = "Director";
            dgvLista.Columns["episodios"].HeaderText = "Episodios";
            dgvLista.Columns["fechaEstreno"].HeaderText = "Fecha de Estreno";
            //dgvLista.Columns["usuarioRegistro"].HeaderText = "Usuario Registro";
            //dgvLista.Columns["fechaRegistro"].HeaderText = "Fecha Registro";
            if (lista.Count > 0) dgvLista.CurrentCell = dgvLista.Rows[0].Cells["titulo"];
            btnEditar.Enabled = lista.Count > 0;
            btnEliminar.Enabled = lista.Count > 0;
        }

        private void FrmSerie_Load(object sender, EventArgs e)
        {
            Size = new Size(835, 362);
            listar();
            dtpFechaEstreno.MaxDate = DateTime.Now.Date;
        }

        private void limpiar()
        {
            txtTitulo.Clear();
            txtSinopsis.Clear();
            txtDirector.Clear();
            //cbxUnidadMedida.SelectedIndex = -1;
            nudEpisodios.Value = 1;
            dtpFechaEstreno.Value = DateTime.Now.Date;
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Size = new Size(835, 362);
            limpiar();
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            esNuevo = true;
            Size = new Size(835, 487);
            limpiar(); // Agregar esta línea para limpiar los campos
            txtTitulo.Focus();
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            esNuevo = false;
            Size = new Size(835, 487);

            int index = dgvLista.CurrentRow.Index;
            int id = Convert.ToInt32(dgvLista.Rows[index].Cells["id"].Value);
            var serie = SerieCln.obtenerUno(id);
            txtTitulo.Text = serie.titulo;
            txtSinopsis.Text = serie.sinopsis;
            txtDirector.Text = serie.director;
            nudEpisodios.Value = serie.episodios;
            if (serie.fechaEstreno != null &&
                    serie.fechaEstreno >= dtpFechaEstreno.MinDate &&
                    serie.fechaEstreno <= dtpFechaEstreno.MaxDate)
            {
                dtpFechaEstreno.Value = serie.fechaEstreno;
            }
            else
            {
                // Si la fecha no es válida, usar una fecha por defecto
                dtpFechaEstreno.Value = new DateTime(2020, 1, 1);
            }
            txtTitulo.Focus();
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            listar();
        }

        private void txtParametro_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter) listar();
        }

        private bool validar()
        {
            bool esValido = true;
            erpTitulo.SetError(txtTitulo, "");
            erpSinopsis.SetError(txtSinopsis, "");
            erpDirector.SetError(txtDirector, "");
            erpEpisodios.SetError(nudEpisodios, "");

            if (string.IsNullOrEmpty(txtTitulo.Text))
            {
                erpTitulo.SetError(txtTitulo, "El campo título es obligatorio.");
                esValido = false;
            }
            if (string.IsNullOrEmpty(txtSinopsis.Text))
            {
                erpSinopsis.SetError(txtSinopsis, "El campo sinopsis es obligatorio.");
                esValido = false;
            }
            if (string.IsNullOrEmpty(txtDirector.Text))
            {
                erpDirector.SetError(txtDirector, "El campo director es obligatorio.");
                esValido = false;
            }
            if (nudEpisodios.Value <= 0)
            {
                erpEpisodios.SetError(nudEpisodios, "El campo episodios debe ser mayor a cero.");
                esValido = false;
            }

            return esValido;
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (validar())
            {
                var serie = new Serie();
                serie.titulo = txtTitulo.Text.Trim();
                serie.sinopsis = txtSinopsis.Text.Trim();
                serie.director = txtDirector.Text.Trim();
                serie.episodios = (int)nudEpisodios.Value;
                serie.fechaEstreno = dtpFechaEstreno.Value;
                //serie.usuarioRegistro = "admin";

                if (esNuevo)
                {
                    SerieCln.insertar(serie);

                }
                else
                {
                    int index = dgvLista.CurrentRow.Index;
                    int id = Convert.ToInt32(dgvLista.Rows[index].Cells["id"].Value);
                    serie.id = id;
                    SerieCln.actualizar(serie);
                }

                    listar();
                btnCancelar.PerformClick();
                MessageBox.Show("Serie guardada correctamente", "...::: Parcial2Kecm - Mensaje :::...",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            int index = dgvLista.CurrentRow.Index;
            int id = Convert.ToInt32(dgvLista.Rows[index].Cells["id"].Value);
            string titulo = dgvLista.Rows[index].Cells["titulo"].Value.ToString();
            DialogResult result = MessageBox.Show($"¿Está seguro de eliminar la serie '{titulo}'?", "...::: Parcial2Kecm - Mensaje :::...",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                SerieCln.eliminar(id);
                listar();
                MessageBox.Show("Serie eliminada correctamente", "...::: Parcial2Kecm - Mensaje :::...",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
