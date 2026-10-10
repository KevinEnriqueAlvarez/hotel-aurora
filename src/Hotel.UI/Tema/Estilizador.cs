using System.Reflection;
using Hotel.UI.Controles;

namespace Hotel.UI.Tema;

/// <summary>Aplica el tema a tablas, campos y formularios.</summary>
/// <remarks>RNF-05; sección 10.3</remarks>
public static class Estilizador
{
    /// <summary>Columnas con este valor en <c>Tag</c> (o llamadas «Estado») se pintan como insignia.</summary>
    public const string EtiquetaInsignia = "insignia";

    /// <summary>
    /// Tabla sin bordes ni encabezados de fila, alto 36, filas alternas, selección SeleccionTabla,
    /// FullRowSelect y solo lectura.
    /// </summary>
    public static void AplicarTabla(DataGridView tabla)
    {
        tabla.BorderStyle = BorderStyle.None;
        tabla.CellBorderStyle = DataGridViewCellBorderStyle.None;
        tabla.BackgroundColor = Paleta.Superficie;
        tabla.GridColor = Paleta.Borde;
        tabla.RowHeadersVisible = false;
        tabla.AllowUserToAddRows = false;
        tabla.AllowUserToDeleteRows = false;
        tabla.AllowUserToResizeRows = false;
        tabla.ReadOnly = true;
        tabla.MultiSelect = false;
        tabla.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        tabla.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        tabla.RowTemplate.Height = 36;
        tabla.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
        tabla.ColumnHeadersHeight = 40;
        tabla.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
        tabla.EnableHeadersVisualStyles = false;
        tabla.Font = Tipografia.Texto;

        var encabezado = tabla.ColumnHeadersDefaultCellStyle;
        encabezado.BackColor = Paleta.Superficie;
        encabezado.ForeColor = Paleta.Marino;
        encabezado.Font = Tipografia.Etiqueta;
        encabezado.SelectionBackColor = Paleta.Superficie;
        encabezado.SelectionForeColor = Paleta.Marino;
        encabezado.Padding = new Padding(8, 0, 8, 0);
        encabezado.Alignment = DataGridViewContentAlignment.MiddleLeft;

        var celda = tabla.DefaultCellStyle;
        celda.BackColor = Paleta.Superficie;
        celda.ForeColor = Paleta.TextoPrincipal;
        celda.SelectionBackColor = Paleta.SeleccionTabla;
        celda.SelectionForeColor = Paleta.TextoPrincipal;
        celda.Padding = new Padding(8, 0, 8, 0);

        tabla.AlternatingRowsDefaultCellStyle.BackColor = Paleta.Tarjeta;
        tabla.AlternatingRowsDefaultCellStyle.SelectionBackColor = Paleta.SeleccionTabla;
        tabla.AlternatingRowsDefaultCellStyle.SelectionForeColor = Paleta.TextoPrincipal;

        ActivarDobleBufer(tabla);
        tabla.CellPainting -= PintarInsignia;
        tabla.CellPainting += PintarInsignia;
    }

    /// <summary>Hace planos y consistentes los campos de entrada de un contenedor (recursivo).</summary>
    public static void AplicarCampos(Control raiz)
    {
        foreach (Control c in raiz.Controls)
        {
            switch (c)
            {
                case TextBox t:
                    t.BorderStyle = BorderStyle.FixedSingle;
                    t.BackColor = Paleta.Superficie;
                    t.ForeColor = Paleta.TextoPrincipal;
                    t.Font = Tipografia.Texto;
                    break;
                case ComboBox cb:
                    cb.FlatStyle = FlatStyle.Flat;
                    cb.BackColor = Paleta.Superficie;
                    cb.ForeColor = Paleta.TextoPrincipal;
                    cb.Font = Tipografia.Texto;
                    break;
                case DateTimePicker dt:
                    dt.Font = Tipografia.Texto;
                    dt.CalendarMonthBackground = Paleta.Superficie;
                    dt.CalendarTitleBackColor = Paleta.Marino;
                    dt.CalendarTitleForeColor = Color.White;
                    dt.CalendarForeColor = Paleta.TextoPrincipal;
                    break;
                case NumericUpDown n:
                    n.BorderStyle = BorderStyle.FixedSingle;
                    n.BackColor = Paleta.Superficie;
                    n.ForeColor = Paleta.TextoPrincipal;
                    n.Font = Tipografia.Texto;
                    break;
                case CheckBox ch:
                    ch.ForeColor = Paleta.TextoPrincipal;
                    ch.Font = Tipografia.Texto;
                    break;
                case Label l when l.Tag as string != "personalizada":
                    l.ForeColor = l.Tag as string == "secundaria" ? Paleta.TextoSecundario : l.ForeColor;
                    break;
            }
            if (c.HasChildren) AplicarCampos(c);
        }
    }

    /// <summary>Pinta la insignia de estado en las columnas marcadas.</summary>
    private static void PintarInsignia(object? sender, DataGridViewCellPaintingEventArgs e)
    {
        if (sender is not DataGridView tabla || e.RowIndex < 0 || e.ColumnIndex < 0) return;
        var columna = tabla.Columns[e.ColumnIndex];
        if (columna.Name != "Estado" && columna.Tag as string != EtiquetaInsignia) return;

        e.Paint(e.CellBounds, DataGridViewPaintParts.All & ~DataGridViewPaintParts.ContentForeground);
        var graficos = e.Graphics;
        if (graficos is null) return;
        GraficosUtil.Suavizar(graficos);
        InsigniaEstado.Pintar(graficos, e.CellBounds with { X = e.CellBounds.X + 6, Width = e.CellBounds.Width - 12 },
            Convert.ToString(e.Value) ?? "");
        e.Handled = true;
    }

    private static void ActivarDobleBufer(DataGridView tabla) =>
        typeof(DataGridView).GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic)
            ?.SetValue(tabla, true);
}
