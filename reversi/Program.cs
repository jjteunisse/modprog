using System.Drawing;
using System.Windows.Forms;

class Tile : Panel {
    public Tile(Size _s, Point _l) { Size = _s; Location = _l; }
    public Color c;
}

class Reversi : Form {
    bool help = false;
    Color mc;
    int gw = 6, gh = 6, sqs = 50;
    Label s = new Label();
    Tile[,] tls;
    Reversi() {
        tls = new Tile[gw, gh];
        for (int i = 0; i < gw; i++) for (int j = 0; j < gh; j++) {
            tls[i, j] = new Tile(new Size(sqs + 1, sqs + 1), new Point(i * sqs, j * sqs));
            tls[i, j].Paint += (o, pea) => DrawTile((Tile)o, pea);
            tls[i, j].MouseClick += (o, mea) => TileClicked((Tile)o, mea.X, mea.Y);
            Controls.Add(tls[i, j]);
        }
        CreateGUI();
        Reset();
    }
    void CreateGUI() {
        Button h = new Button(), r = new Button();
        h.ForeColor = r.ForeColor = Color.WhiteSmoke;
        
        h.Click += (o, ea) => { help = !help; UpdateMoves(); };
        h.Location = new Point(gw * sqs + 1 - h.Width, gh * sqs + 2);
        h.Text = "Help";
        
        r.Click += (o, ea) => Reset();
        r.Location = new Point(0, gh * sqs + 2);
        r.Text = "Reset";

        s.Location = new Point(0, gh * sqs + 2 + r.Height);
        s.Size = new Size(gw * sqs + 1, s.Height);
        s.TextAlign = ContentAlignment.MiddleCenter;

        BackColor = Color.Black;
        ClientSize = new Size(gw * sqs + 1, gh * sqs + 1 + r.Height + s.Height);
        FormBorderStyle = FormBorderStyle.FixedSingle;
        Text = "Reversi";

        Controls.Add(h); Controls.Add(r); Controls.Add(s);
    }
    void DrawTile(Tile t, PaintEventArgs pea) {
        pea.Graphics.DrawRectangle(Pens.WhiteSmoke, 0, 0, sqs, sqs);
        if (t.c == Color.LightGreen) { if (help) pea.Graphics.DrawEllipse(Pens.LightGreen, 0, 0, sqs, sqs); }
        else if (t.c != Color.Empty) pea.Graphics.FillEllipse(new SolidBrush(t.c), 0, 0, sqs, sqs);
    }
    void Reset() {
        for (int i = 0; i < gw; i++) for (int j = 0; j < gh; j++) {
            tls[i, j].c = Color.Empty;
            tls[i, j].Invalidate();
        }
        tls[gw / 2 - 1, gh / 2 - 1].c = tls[gw / 2, gh / 2].c = Color.Aqua;
        tls[gw / 2, gh / 2 - 1].c = tls[gw / 2 - 1, gh / 2].c = Color.Coral;
        mc = Color.Empty;
        UpdateStatus();
    }
    void TileClicked(Tile t, int x, int y) {
        if (x != 0 && x != sqs && y != 0 && y != sqs
            && ValidMove(t.Location.X / sqs, t.Location.Y / sqs, true)) {
            UpdateStatus();
        }
    }
    void UpdateMoves() {
        for (int i = 0; i < gw; i++) for (int j = 0; j < gh; j++) {
            if (tls[i, j].c == Color.LightGreen) { tls[i, j].c = Color.Empty; tls[i, j].Invalidate(); }
            if (help && ValidMove(i, j, false)) { tls[i, j].c = Color.LightGreen; tls[i, j].Invalidate(); }
        }
    }
    bool SwitchStatus() {
        mc = (mc == Color.Aqua) ? Color.Coral : Color.Aqua;
        UpdateMoves();
        for (int i = 0; i < gw; i++) for (int j = 0; j < gh; j++) {
            if ((help && tls[i, j].c == Color.LightGreen) || (!help && ValidMove(i, j, false))) {
                s.Text = mc.Name + "'s turn";
                s.ForeColor = mc;
                return true;
            }
        }
        return false;
    }
    void UpdateStatus() {
        if (!SwitchStatus() && !SwitchStatus()) {
            int d = 0;
            for (int i = 0; i < gw; i++) for (int j = 0; j < gh; j++) {
                if (tls[i, j].c == Color.Aqua) d++;
                else if (tls[i, j].c == Color.Coral) d--;
            }
            if (d == 0) { s.Text = "The game ended in a draw"; s.ForeColor = Color.WhiteSmoke; }
            else if (d > 0) { s.Text = "Aqua wins by " + d + " disks"; s.ForeColor = Color.Aqua; }
            else { s.Text = "Coral wins by " + -d + " disks"; s.ForeColor = Color.Coral; }
        }
    }
    bool ValidMove(int x, int y, bool draw) {
        if (tls[x, y].c == Color.Empty || tls[x, y].c == Color.LightGreen) {
            for (int i = -1; i <= 1; i++) for (int j = -1; j <= 1; j++) {
                int cx = x + i, cy = y + j;
                int cvt = 0;
                while (cx >= 0 && cy >= 0 && cx < gw && cy < gh
                       && tls[cx, cy].c != Color.Empty && tls[cx, cy].c != Color.LightGreen) {
                    if (tls[cx, cy].c != mc) { cx += i; cy += j; cvt++; }
                    else {
                        if (cvt > 0) {
                            if (!draw) return true;
                            for (int k = 0; k <= cvt; k++) {
                                tls[x + k * i, y + k * j].c = mc;
                                tls[x + k * i, y + k * j].Invalidate();
                            }
                        }
                        break;
                    } 
                }
            }
            return tls[x, y].c == mc;
        }
        return false;
    }
    static void Main() { Application.Run(new Reversi()); }
}