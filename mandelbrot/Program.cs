using System;
using System.Drawing;
using System.Windows.Forms;

class Mandelbrot : Form {
    Button ok = new Button() { Size = new Size(35, 35), Location = new Point(275, 45), Text = "OK" };
    Label cxlb = new Label() { Size = new Size(100, 15), Location = new Point(25, 10), Text = "Center X" };
    Label cylb = new Label() { Size = new Size(100, 15), Location = new Point(25, 60), Text = "Center Y" };
    Label sclb = new Label() { Size = new Size(100, 15), Location = new Point(150, 10), Text = "Scale" };
    Label mflb = new Label() { Size = new Size(100, 15), Location = new Point(150, 60), Text = "Max F" };
    ListBox cases = new ListBox() { Size = new Size(90, 75), Location = new Point(335, 25) };
    PictureBox pb = new PictureBox() { Size = new Size(400, 400), Location = new Point(25, 150) };
    TextBox cxtb = new TextBox() { Size = new Size(100, 25), Location = new Point(25, 25), Text = "0.0" };
    TextBox cytb = new TextBox() { Size = new Size(100, 25), Location = new Point(25, 75), Text = "0.0" };
    TextBox sctb = new TextBox() { Size = new Size(100, 25), Location = new Point(150, 25), Text = "0.01" };
    TextBox mftb = new TextBox() { Size = new Size(100, 25), Location = new Point(150, 75), Text = "100" };
    double cx = 0.0, cy = 0.0, sc = 0.01;
    int mf = 100;
    Mandelbrot() {
        cases.Items.Add("base"); cases.Items.Add("star"); cases.Items.Add("creed"); cases.Items.Add("mini");
        cases.Click += CasesClicked;
        ok.Click += OkClicked;
        pb.Click += PbClicked;
        pb.Paint += DrawM;
        this.Size = new Size(450, 600);
        this.Controls.Add(cxlb); this.Controls.Add(cylb); this.Controls.Add(sclb); this.Controls.Add(mflb);
        this.Controls.Add(cxtb); this.Controls.Add(cytb); this.Controls.Add(sctb); this.Controls.Add(mftb);
        this.Controls.Add(ok); this.Controls.Add(cases); this.Controls.Add(pb);
    }
    int CalcM(double x, double y) {
        double a = 0.0f, b = 0.0f;
        int m;
        for (m = 1; m < mf; m++) {
            double ta = a * a - b * b + x;
            double tb = 2 * a * b + y;
            if (Math.Sqrt(ta * ta + tb * tb) >= 2.0) break;
            a = ta;
            b = tb;
        }
        return m;
    }
    void CasesClicked(object o, EventArgs ea) {
        if (cases.SelectedIndex == 0) { cx = 0.0; cy = 0.0; sc = 1E-2; }
        else if (cases.SelectedIndex == 1) { cx = 0.14271; cy = -0.63067; sc = 2E-6; }
        else if (cases.SelectedIndex == 2) { cx = -0.00274; cy = -0.74507; sc = 2E-6; }
        else if (cases.SelectedIndex == 3) { cx = -1.7625; cy = 0.0; sc = 1E-4; }
        pb.Refresh();
    }
    void PbClicked(object o, EventArgs ea) {
        cx = cx + ((ea as MouseEventArgs).X - 200) * sc;
        cy = cy + ((ea as MouseEventArgs).Y - 200) * sc;
        sc /= 2;
        pb.Refresh();
    }
    void OkClicked(object o, EventArgs ea) {
        cx = double.Parse(cxtb.Text);
        cy = double.Parse(cytb.Text);
        sc = double.Parse(sctb.Text);
        mf = int.Parse(mftb.Text);
        pb.Refresh();
    }
    void DrawM(object o, PaintEventArgs pea) {
        double y = cy - 200 * sc;
        for (int i = 0; i < 400; i++) {
            double x = cx - 200 * sc;
            for (int j = 0; j < 400; j++) {
                int m = CalcM(x, y);
                int g = ((m * 255 / mf % 2) == 0) ? m * 255 / mf : 0;
                Brush br = (m == mf) ? Brushes.Black : new SolidBrush(Color.FromArgb(0, g, (m * 255) / mf));
                pea.Graphics.FillRectangle(br, j, i, 1, 1);
                x += sc;
            }
            y += sc;
        }
        cxtb.Text = cx.ToString("0.##########");
        cytb.Text = cy.ToString("0.##########");
        sctb.Text = sc.ToString("0.##########");
    }
    static void Main() { Application.Run(new Mandelbrot()); }
}