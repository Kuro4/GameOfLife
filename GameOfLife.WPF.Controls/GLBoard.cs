using GameOfLife.Core.Models;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms.Integration;

namespace GameOfLife.WPF.Controls
{
    public class GLBoard : Control
    {
        public IBindableBoard Board
        {
            get { return (IBindableBoard)GetValue(BoardProperty); }
            set { SetValue(BoardProperty, value); }
        }
        // Using a DependencyProperty as the backing store for Board.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BoardProperty =
            DependencyProperty.Register("Board", typeof(IBindableBoard), typeof(GLBoard), new PropertyMetadata(null, BoardPropertyChanged));

        private static void BoardPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var self = (GLBoard)d;
            if (e.OldValue is IBindableBoard oldBoard)
            {
                oldBoard.Initialized -= self.Board_Initialized;
                oldBoard.CellLifeChanged -= self.Board_CellLifeChanged;
                oldBoard.NextRise -= self.Board_NextRise;
            }
            if (e.NewValue is IBindableBoard newBoard)
            {
                newBoard.Initialized += self.Board_Initialized;
                newBoard.CellLifeChanged += self.Board_CellLifeChanged;
                newBoard.NextRise += self.Board_NextRise;
            }
            self.SetProjection();
        }

        private void Board_Initialized(object sender, EventArgs e)
        {
            this.SetProjection();
        }

        private void Board_CellLifeChanged(object sender, EventArgs e)
        {
            this.RenderBoard();
        }

        private void Board_NextRise(object sender, EventArgs e)
        {
            this.RenderBoard();
        }

        private Vector2[] vec2;//ポイント
        private Color4[] col4;//色
        private readonly int[] vbos = new int[2];//バッファ

        private WindowsFormsHost glHost;
        private GLControl glControl;

        static GLBoard()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(GLBoard), new FrameworkPropertyMetadata(typeof(GLBoard)));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (this.glControl != null)
            {
                //イベント購読解除
                this.glHost.Loaded -= this.GlHost_Loaded;
                this.glHost.Unloaded -= this.GlHost_Unloaded;
            }

            this.glHost = this.GetTemplateChild("PART_Host") as WindowsFormsHost;
            this.glControl = glHost.FindName("PART_GLControl") as GLControl;

            if(this.glControl != null)
            {
                //イベント購読
                this.glHost.Loaded += this.GlHost_Loaded;
                this.glHost.Unloaded += this.GlHost_Unloaded;
            }
        }        

        private void GlHost_Loaded(object sender, RoutedEventArgs e)
        {
            //OpenTKの使用準備
            //描画領域の設定
            GL.Viewport(this.glControl.Size);
            //背景色の設定
            GL.ClearColor(Color4.Black);
            //各Arrayを有効化
            GL.EnableClientState(ArrayCap.VertexArray);
            GL.EnableClientState(ArrayCap.ColorArray);
            //バッファの生成
            GL.GenBuffers(2, this.vbos);
            //イベント購読
            this.glControl.Resize += this.GlControl_Resize;
            this.glControl.Paint += this.GlControl_Paint;
            //描画
            this.RenderBoard();
        }

        private void GlHost_Unloaded(object sender, RoutedEventArgs e)
        {
            //OpenTKの後始末
            //バッファ削除
            GL.DeleteBuffers(2, this.vbos);
            //各Arrayの無効化
            GL.DisableClientState(ArrayCap.VertexArray);
            GL.DisableClientState(ArrayCap.ColorArray);
            //イベント購読解除
            this.glControl.Paint -= this.GlControl_Paint;
            this.glControl.Resize -= this.GlControl_Resize;
        }

        private void GlControl_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            if (this.Board == null) return;
            GL.Clear(ClearBufferMask.ColorBufferBit);

            //描画処理
            //頂点の位置情報の場所を指定
            GL.BindBuffer(BufferTarget.ArrayBuffer, this.vbos[0]);
            GL.VertexPointer(2, VertexPointerType.Float, 0, 0);
            //頂点の色情報の場所を指定
            GL.BindBuffer(BufferTarget.ArrayBuffer, this.vbos[1]);
            GL.ColorPointer(4, ColorPointerType.Float, 0, 0);
            //点のサイズ指定
            GL.PointSize(5F);
            //点を描画
            GL.DrawArrays(PrimitiveType.Points, 0, this.vec2.Length);
            //バッファの紐付けを解除
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

            this.glControl.SwapBuffers();
        }

        private void GlControl_Resize(object sender, EventArgs e)
        {
            GL.Viewport(((GLControl)sender).Size);
            this.SetProjection();
        }

        private void SetProjection()
        {
            //視点設定
            GL.MatrixMode(MatrixMode.Modelview);
            var col = ((this.Board.ColumnCount + 1) / 2F) * this.glControl.AspectRatio;
            var row = (this.Board.RowCount + 1) / -2F;
            Matrix4 look = Matrix4.LookAt(new Vector3(col, row, 1.0f), new Vector3(col, row, 0.0f), Vector3.UnitY);
            GL.LoadMatrix(ref look);
            //視界設定
            GL.MatrixMode(MatrixMode.Projection);
            float h = this.Board.RowCount + 1;
            float w = h * this.glControl.AspectRatio;
            Matrix4 proj = Matrix4.CreateOrthographic(w, h, 0.01f, 2.0f);
            GL.LoadMatrix(ref proj);
            this.RenderBoard();
        }
        /// <summary>
        /// 現在の<see cref="IBindableBoard"/>の状態をレンダリングする
        /// </summary>
        private void RenderBoard()
        {
            if (this.Board == null) return;

            var count = this.Board.ColumnCount * this.Board.RowCount;
            this.vec2 = new Vector2[count];
            this.col4 = new Color4[count];

            foreach ((ICell cell, int i) in this.Board.Cells.Select((x, i) => (x, i)))
            {   
                //Y座標は反転させる
                this.vec2[i] = new Vector2((cell.X + 1) * this.glControl.AspectRatio, (cell.Y * -1) - 1);
                this.col4[i] = cell.IsAlive ? Color4.White : Color4.Black;
            }
            //ポイントデータを流す
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbos[0]);
            int positionArraySize = vec2.Length * Marshal.SizeOf(default(Vector2));
            GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(positionArraySize), vec2, BufferUsageHint.StaticDraw);
            //色データを流す
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbos[1]);
            int colorArraySize = col4.Length * Marshal.SizeOf(default(Color4));
            GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(colorArraySize), col4, BufferUsageHint.StaticDraw);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

            this.glControl?.Invalidate();
        }
    }
}
