using System.Windows.Media;
using System.Windows;

namespace SLControls.Book.Helpers
{
    struct PageParameters
    {
        public PageParameters(Size renderSize) 
        {
            m_page0ShadowOpacity = 0;
            m_page0ShadowEndPoint = new Point();
            m_page0ShadowStartPoint = new Point();
            m_page1ClippingFigure = new PathFigure
                                        {
                                            Segments = new PathSegmentCollection()
                                        };

            m_page1ReflectionEndPoint = new Point();
            m_page1ReflectionStartPoint = new Point();
            m_page1RotateAngle = 0;
            m_page1RotateCenterX = 0;
            m_page1RotateCenterY = 0;
            m_page1TranslateX = 0;
            m_page1TranslateY = 0;
            m_page2ClippingFigure = new PathFigure();
            m_page2ClippingFigure.Segments = new PathSegmentCollection();

            m_renderSize = renderSize;
        }

        public double Page0ShadowOpacity
        {
            get { return m_page0ShadowOpacity; }
            set { m_page0ShadowOpacity = value; }
        }
        private double m_page0ShadowOpacity;

        public double Page1RotateAngle
        {
            get { return m_page1RotateAngle; }
            set { m_page1RotateAngle = value; }
        }
        private double m_page1RotateAngle;

        public double Page1RotateCenterX
        {
            get { return m_page1RotateCenterX; }
            set { m_page1RotateCenterX = value; }
        }
        private double m_page1RotateCenterX;

        public double Page1RotateCenterY
        {
            get { return m_page1RotateCenterY; }
            set { m_page1RotateCenterY = value; }
        }
        private double m_page1RotateCenterY;

        public double Page1TranslateX
        {
            get { return m_page1TranslateX; }
            set { m_page1TranslateX = value; }
        }
        private double m_page1TranslateX;

        public double Page1TranslateY
        {
            get { return m_page1TranslateY; }
            set { m_page1TranslateY = value; }
        }
        private double m_page1TranslateY;

        public PathFigure Page1ClippingFigure
        {
            get { return m_page1ClippingFigure; }
            set { m_page1ClippingFigure = value; }
        }
        private PathFigure m_page1ClippingFigure;

        public PathFigure Page2ClippingFigure
        {
            get { return m_page2ClippingFigure; }
            set { m_page2ClippingFigure = value; }
        }
        private PathFigure m_page2ClippingFigure;

        public Point Page1ReflectionStartPoint
        {
            get { return m_page1ReflectionStartPoint; }
            set { m_page1ReflectionStartPoint = value; }
        }
        private Point m_page1ReflectionStartPoint;

        public Point Page1ReflectionEndPoint
        {
            get { return m_page1ReflectionEndPoint; }
            set { m_page1ReflectionEndPoint = value; }
        }
        private Point m_page1ReflectionEndPoint;

        public Point Page0ShadowStartPoint
        {
            get { return m_page0ShadowStartPoint; }
            set { m_page0ShadowStartPoint = value; }
        }
        private Point m_page0ShadowStartPoint;

        public Point Page0ShadowEndPoint
        {
            get { return m_page0ShadowEndPoint; }
            set { m_page0ShadowEndPoint = value; }
        }
        private Point m_page0ShadowEndPoint;

        public Size RenderSize 
        {
            get { return m_renderSize; }
            set { m_renderSize = value; }
        }
        private Size m_renderSize;
    }
}