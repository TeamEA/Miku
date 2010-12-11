using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Miku.Client.Views.UserControl;
using System.Windows.Media.Media3D;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace Miku.Client.Views.MainViews
{
    /// <summary>
    /// Interaction logic for MainView.xaml
    /// </summary>
    public partial class MainView : Window
    {
        private bool IsSwitch;
        public MainView()
        {
            InitializeComponent();
            IsSwitch = false;
           
        }

        public void MoveCurrentToNext()
        {
            
            var current = this.model3DGroup.Children[0] as GeometryModel3D;
            var left = this.model3DGroup.Children[2] as GeometryModel3D;
            var right = this.model3DGroup.Children[1] as GeometryModel3D;
            this.model3DGroup.Children.RemoveAt(0);
            this.model3DGroup.Children.Add(current);

            
            var transform3DGroup = right.Transform as Transform3DGroup;

            var translate = transform3DGroup.Children[1] as TranslateTransform3D;
            var rotate = (transform3DGroup.Children[0] as RotateTransform3D).Rotation as AxisAngleRotation3D;
            AnimationVisualElement(translate, rotate, 0, 0, 0, 0, true);


            transform3DGroup = current.Transform as Transform3DGroup;

            translate = transform3DGroup.Children[1] as TranslateTransform3D;
            rotate = (transform3DGroup.Children[0] as RotateTransform3D).Rotation as AxisAngleRotation3D;
            AnimationVisualElement(translate, rotate, -1.8, 0, -2.5, 45, true);

            transform3DGroup = left.Transform as Transform3DGroup;

            translate = transform3DGroup.Children[1] as TranslateTransform3D;
            rotate = (transform3DGroup.Children[0] as RotateTransform3D).Rotation as AxisAngleRotation3D;
            AnimationVisualElement(translate, rotate, 1.8, 0, -2.5, -45, true);

        }

        public void MoveCurrentToPrevious()
        {
           
            var current = this.model3DGroup.Children[2] as GeometryModel3D;
            var center = this.model3DGroup.Children[0] as GeometryModel3D;
            var right = this.model3DGroup.Children[1] as GeometryModel3D;
            this.model3DGroup.Children.RemoveAt(2);
            this.model3DGroup.Children.Insert(0, current);

           
            var transform3DGroup = center.Transform as Transform3DGroup;

            var translate = transform3DGroup.Children[1] as TranslateTransform3D;
            var rotate = (transform3DGroup.Children[0] as RotateTransform3D).Rotation as AxisAngleRotation3D;

            AnimationVisualElement(translate, rotate, 1.8, 0, -2.5, -45, true);

            transform3DGroup = current.Transform as Transform3DGroup;

            translate = transform3DGroup.Children[1] as TranslateTransform3D;
            rotate = (transform3DGroup.Children[0] as RotateTransform3D).Rotation as AxisAngleRotation3D;
            AnimationVisualElement(translate, rotate, 0, 0, 0, 0, true);

            transform3DGroup = right.Transform as Transform3DGroup;

            translate = transform3DGroup.Children[1] as TranslateTransform3D;
            rotate = (transform3DGroup.Children[0] as RotateTransform3D).Rotation as AxisAngleRotation3D;
            AnimationVisualElement(translate, rotate, -1.8, 0, -2.5, 45, true);
        }
        private void AnimationVisualElement(TranslateTransform3D translate, AxisAngleRotation3D rotate, double targetX, double targetY, double targetZ, double angel, bool forward)
        {
            Duration duration = new Duration(TimeSpan.FromSeconds(.4));
            
            DoubleAnimation animationX = new DoubleAnimation();
            animationX.To = targetX;
            animationX.Duration = duration;
            animationX.AccelerationRatio = forward ? 0 : 1;
            animationX.DecelerationRatio = forward ? 1 : 0;
            translate.BeginAnimation(TranslateTransform3D.OffsetXProperty, animationX);
            
            DoubleAnimation animationY = new DoubleAnimation();
            animationX.To = targetY;
            animationX.AccelerationRatio = forward ? 0.7 : 0.3;
            animationX.DecelerationRatio = forward ? 0.3 : 0.7;
            animationX.Duration = duration;
            translate.BeginAnimation(TranslateTransform3D.OffsetYProperty, animationX);
           
            DoubleAnimation animationZ = new DoubleAnimation();
            animationZ.To = targetZ;
            animationZ.AccelerationRatio = forward ? 0.3 : 0.7;
            animationZ.DecelerationRatio = forward ? 0.7 : 0.3;
            animationZ.Duration = duration;
            translate.BeginAnimation(TranslateTransform3D.OffsetZProperty, animationZ);
           
            DoubleAnimation animationAngel = new DoubleAnimation();
            animationAngel.To = angel;
            animationAngel.AccelerationRatio = forward ? 0.3 : 0.7;
            animationAngel.DecelerationRatio = forward ? 0.7 : 0.3;
            animationAngel.Duration = duration;
            rotate.BeginAnimation(AxisAngleRotation3D.AngleProperty, animationAngel);
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyStates == Keyboard.GetKeyStates(Key.Left))
            {
              
                this.MoveCurrentToPrevious();
            }
            else if (e.KeyStates == Keyboard.GetKeyStates(Key.Right))
            {                
                this.MoveCurrentToNext();
            }
        }

        private void Window_MouseMove(object sender, MouseEventArgs e)
        {
            double PointX = e.GetPosition(this).X;
            double PointY = e.GetPosition(this).Y;
            this.DoSwitch(PointX,PointY);
        }

        private void DoSwitch(double PointX, double PointY)
        {
            if (PointX < (double) this.Width/3 &&
                PointY > ((double)this.Width / 7) &&
                PointY < ((double)this.Width * 5 / 7))
            {
                if (!IsSwitch)
                {
                    this.MoveCurrentToPrevious();
                    IsSwitch = true;
                }
            }
            else
                if (PointX > (double)this.Width*2 / 3 &&
                    PointY > ((double)this.Width / 7) &&
                    PointY < ((double)this.Width * 5 / 7))
                {
                    if (!IsSwitch)
                    {
                        this.MoveCurrentToNext();
                        IsSwitch = true;
                    }
                }
                else
                    if (PointX < (double)this.Width / 5 &&
                        PointY > ((double)this.Width / 7) &&
                        PointY < ((double)this.Width * 5 / 7))
                    {
                        if (IsSwitch)
                        {
                            this.MoveCurrentToPrevious();
                            IsSwitch = true;
                        }
                    }
                    else
                        if (PointX > (double)this.Width * 4 / 5 &&
                            PointY > ((double)this.Width / 7) &&
                            PointY < ((double)this.Width * 5 / 7))
                        {
                            if (IsSwitch)
                            {
                                this.MoveCurrentToNext();
                                IsSwitch = true;
                            }
                        }
                        else
                       {
                             IsSwitch = false;
                        }
        }
        #region
        protected override void OnClosed(EventArgs e)
        {
            Properties.Settings.Default.Save();
            base.OnClosed(e);
        }
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                IsSwitch = true;
                this.DragMove();

            }
            double PointX = e.GetPosition(this).X;
            double PointY = e.GetPosition(this).Y;
            this.DoSwitch(PointX, PointY);
        }

        private void btnMinSize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = System.Windows.WindowState.Minimized;
        }

        private void btnMaxSize_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            YesNoMessageBox messageBox = new YesNoMessageBox("是否这的要关闭系统？");
            messageBox.ShowDialog();
            if (messageBox.YesOrNo == YesNoMessageBox.YesAndNo.Yes)
            {
                this.Close();
            }
        }
        #endregion
        
    }
}
