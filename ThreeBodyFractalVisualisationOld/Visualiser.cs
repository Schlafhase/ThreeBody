using System.Diagnostics;
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
    private readonly Thread _thread;
    private float _timeSinceStart = 0f;
    
    public float TimeStep { get; set; } = 0.01f;
    
    private SynchronizationContext _syncContext;
    
    private bool _running = true;
    
    public Visualiser(Action? update = null, Canvas.Canvas? canvas = null, float runTime = -1f)
    {
        _syncContext = SynchronizationContext.Current;
        _bodies = ThreeBodySimulator.GenerateStableConfiguration(0);

        PositionedComponent[] bodyComponents =
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
                _timeSinceStart += TimeStep;
                
                if (runTime >= 0 && _timeSinceStart >= runTime)
                {
                    break;
                }
                
                Array.ForEach(_orbits, orbit =>
                {
                    orbit.X = _canvas.Width / 2;
                    orbit.Y = _canvas.Height / 2;
                });
                
                for (int i = 0; i < _bodies.Length; i++)
                {
                    _bodies[i].Position += _bodies[i].Velocity * TimeStep;

                    lock (_orbits[i].PointsLocker)
                    {
                        _orbits[i].Points.Add(new Point((int)(_bodies[i].Position.X), (int)(_bodies[i].Position.Y)));

                        if (_orbits[i].Points.Count > 500)
                        {
                            _orbits[i].Points.RemoveAt(0);
                        }
                    }
                }

                try
                {
                    for (int i = 0; i < _bodies.Length; i++)
                    {
                        int _i = i; // Copy i because i might have changed by the time the lambda is executed
                        _syncContext.Post(_ => bodyComponents[_i].X = (int)(_bodies[_i].Position.X + _canvas.Width / 2), null);
                        _syncContext.Post(_ => bodyComponents[_i].Y = (int)(_bodies[_i].Position.Y + _canvas.Height / 2), null);
                    }
                }
                catch (ObjectDisposedException)
                {
                    break;
                }

                NOP(0.001);
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
    
    private static void NOP(double durationSeconds)
    {
        var durationTicks = Math.Round(durationSeconds * Stopwatch.Frequency);
        var sw = Stopwatch.StartNew();

        while (sw.ElapsedTicks < durationTicks)
        {

        }
    }
}