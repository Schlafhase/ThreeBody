using System.Numerics;
using Canvas.Components;
using Canvas.Components.Interfaces;
using ThreeBody;
using ThreeBody.Physics;

namespace ThreeBodyVisualisation;

public class Visualiser : IDisposable
{
    private PhysicsBody[] _bodies;
    private BezierCurve[] _orbits;
    private Canvas.Canvas _canvas;
    private Thread _thread;
    
    public float TimeStep { get; set; } = 0.01f;
    
    private SynchronizationContext _syncContext;
    
    private bool _running = true;
    
    public Visualiser(Action? update = null, Canvas.Canvas? canvas = null)
    {
        _syncContext = SynchronizationContext.Current;
        _bodies = ThreeBodySimulator.GenerateStableConfiguration(0);

        IPositionedComponent[] bodyComponents =
        [
            new GlowDot(0, 0, 10, 80, Color.Red),
            new GlowDot(0, 0, 10, 80, Color.Yellow),
            new GlowDot(0, 0, 10, 80, Color.Blue),
        ];

        _orbits =
        [
            new BezierCurve([]),
            new BezierCurve([]),
            new BezierCurve([])
        ];
        Array.ForEach(_orbits, orbit => orbit.Pen = Pens.White);
        
        _canvas = canvas ?? new Canvas.Canvas(800, 800);
        Array.ForEach(_orbits, _canvas.AddChild);
        Array.ForEach(bodyComponents, _canvas.AddChild);
        
        _canvas.BackgroundColor = Color.Black;
        _canvas.OnUpdate = update;
        
        _thread = new Thread(() =>
        {
            while (_running)
            {
                Gravity.SimulateGravity(_bodies, TimeStep);
                
                Array.ForEach(_orbits, orbit => orbit.Offset = new Point(_canvas.Width / 2, _canvas.Height / 2));
                
                for (int i = 0; i < _bodies.Length; i++)
                {
                    _bodies[i].Position += _bodies[i].Velocity * TimeStep;
                    _orbits[i].Points.Add(new Point((int)(_bodies[i].Position.X), (int)(_bodies[i].Position.Y)));
                    
                    if (_orbits[i].Points.Count > 200)
                    {
                        _orbits[i].Points.RemoveAt(0);
                    }
                }

                try
                {
                    for (int i = 0; i < _bodies.Length; i++)
                    {
                        int _i = i;
                        _syncContext.Post(_ => bodyComponents[_i].X = (int)(_bodies[_i].Position.X + _canvas.Width / 2), null);
                        _syncContext.Post(_ => bodyComponents[_i].Y = (int)(_bodies[_i].Position.Y + _canvas.Height / 2), null);
                    }
                }
                catch (ObjectDisposedException)
                {
                    break;
                }

                Thread.Sleep(2);
            }
        });
    }
    
    public void Start()
    {
        _thread.Start();
    }
    
    public void SetConfig(PhysicsBody[] bodies)
    {
        if (bodies.Length != 3)
        {
            throw new ArgumentException("Three bodies are required for this simulation.");
        }
        _bodies = bodies;
    }
    
    public void Dispose()
    {
        _running = false;
        GC.SuppressFinalize(this);
    }
}