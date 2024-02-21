using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ajedrez
{

    class ajedrez
    {
        public const char verde = 'V';
        public const char magenta = 'M';
        public const char vacio = ' ';
        public const char torre = 'T';
        public const char caballo = 'C';
        public const char alfil = 'A';
        public const char dama = 'D';
        public const char rey = 'R';
        public const char peon = 'P';
        public const char movimiento = 'G';
        public const char jaque = 'J';
        [Flags]
        enum Enrocar
        {
            Ninguno = 0,
            MagentaLargo = 1,
            MagentaCorto = 2,
            VerdeLargo = 4,
            VerdeCorto = 8,
        }
        ///////////////////////////////////// PEDIR DATOS
        static int PedirFila()
        {
            int fila = -1;
            try
            {
                fila = (Convert.ToInt32(Console.ReadLine() ?? "0")) - 1;
            }
            catch (FormatException)
            {
                fila = -1;
            }
            return fila;
        }
        static int LetraColumnaANumero()
        {
            string columnaEnLetra = (Console.ReadLine() ?? "0").ToUpper();
            int columna;
            switch (columnaEnLetra)
            {
                case "A":
                    columna = 0;
                    break;
                case "B":
                    columna = 0;
                    break;
                case "C":
                    columna = 0;
                    break;
                case "D":
                    columna = 0;
                    break;
                case "E":
                    columna = 0;
                    break;
                case "F":
                    columna = 0;
                    break;
                case "G":
                    columna = 0;
                    break;
                case "H":
                    columna = 0;
                    break;
                default:
                    columna = -1;
                    break;
            };
            return columna;
        }
        ///////////////////////////////////// TRIGGERS AL MOVER
        static void Promocion(char[,,] tablero, int fila, int columna)
        {
            char promocion = vacio;
            Console.WriteLine("Puedes promocionar tu peon a: (D)ama, (T)orre, (C)aballo" +
                                " o (A)lfil.");
            do
            {
                Console.Write("Promocionar a: ");
                switch ((Console.ReadLine() ?? "0").ToUpper())
                {
                    case "D":
                        promocion = dama;
                        break;
                    case "T":
                        promocion = torre;
                        break;
                    case "C":
                        promocion = caballo;
                        break;
                    case "A":
                        promocion = alfil;
                        break;
                    default:
                        promocion = vacio;
                        break;
                };
            } while (promocion == vacio);
            tablero[fila, columna, 0] = promocion;
        }
        ///////////////////////////////////// JAQUES
        static void ComprobarJaque(char[,,] tablero)
        {
            LimpiarPosiblesMovimientos(tablero);
            for (int i = 0; i < tablero.GetLength(0); i++)
            {
                for (int j = 0; j < tablero.GetLength(1); j++)
                {
                    char colorJugador = tablero[i, j, 1];
                    switch (tablero[i, j, 0])
                    {
                        case peon:
                            PosibleMovimientoPeon(tablero, i, j, colorJugador, true);
                            break;
                        case caballo:
                            PosibleMovimientoCaballo(tablero, i, j, colorJugador, true);
                            break;
                        case alfil:
                            PosibleMovimientoAlfil(tablero, i, j, colorJugador, true);
                            break;
                        case torre:
                            PosibleMovimientoTorre(tablero, i, j, colorJugador, true);
                            break;
                        case dama:
                            PosibleMovimientoAlfil(tablero, i, j, colorJugador, true);
                            PosibleMovimientoTorre(tablero, i, j, colorJugador, true);
                            break;
                    };
                }
            }
        }
        static bool FinPartida(char[,,] tablero, char colorJugador, Enrocar enroques)
        {
            bool finPartida = true;
            bool tablas = true;
            char[,,] tableroTemporal = new char[tablero.GetLength(0), tablero.GetLength(1), tablero.GetLength(2)];
            Array.Copy(tablero, tableroTemporal, tableroTemporal.Length);
            for (int i = 0; i < tableroTemporal.GetLength(0); i++)
            {
                for (int j = 0; j < tableroTemporal.GetLength(1); j++)
                {
                    if (tablas == true && tableroTemporal[i, j, 2] == jaque)
                        tablas = false;
                    if (tableroTemporal[i, j, 1] == colorJugador)
                    {
                        switch (tableroTemporal[i, j, 0])
                        {
                            case peon:
                                tableroTemporal[i, j, 2] = movimiento;
                                PosibleMovimientoPeon(tableroTemporal, i, j, colorJugador);
                                break;
                            case caballo:
                                tableroTemporal[i, j, 2] = movimiento;
                                PosibleMovimientoCaballo(tableroTemporal, i, j, colorJugador);
                                break;
                            case alfil:
                                tableroTemporal[i, j, 2] = movimiento;
                                PosibleMovimientoAlfil(tableroTemporal, i, j, colorJugador);
                                break;
                            case torre:
                                tableroTemporal[i, j, 2] = movimiento;
                                PosibleMovimientoTorre(tableroTemporal, i, j, colorJugador);
                                break;
                            case dama:
                                tableroTemporal[i, j, 2] = movimiento;
                                PosibleMovimientoAlfil(tableroTemporal, i, j, colorJugador);
                                PosibleMovimientoTorre(tableroTemporal, i, j, colorJugador);
                                break;
                            case rey:
                                tableroTemporal[i, j, 2] = movimiento;
                                PosibleMovimientoRey(tableroTemporal, i, j, colorJugador, enroques);
                                break;
                        }
                    }
                }
            }
            for (int i = 0; i < tableroTemporal.GetLength(0) && finPartida == true; i++)
            {
                for (int j = 0; j < tableroTemporal.GetLength(1) && finPartida == true; j++)
                {
                    if (tableroTemporal[i, j, 2] == movimiento && (tableroTemporal[i, j, 1] == vacio || tableroTemporal[i, j, 1] != colorJugador))
                        finPartida = false;
                }
            }
            if (finPartida == true)
            {
                if (tablas == false)
                {
                    Console.WriteLine("¡JAQUE MATE!");
                    Console.WriteLine($"Gana el jugador {(colorJugador == verde ? "magenta." : "verde.")}");
                }
                if (tablas == true)
                {
                    Console.WriteLine("¡TABLAS!");
                    Console.WriteLine($"El rey del jugador {(colorJugador == verde ? "magenta" : "verde")} está ahogado.");
                }
                Console.Write("Pulsa cualquier tecla para terminar la partida.");
                Console.ReadKey();
            }
            LimpiarPosiblesMovimientos(tableroTemporal);
            ComprobarJaque(tableroTemporal);
            return finPartida;
        }
        static bool ComprobarSiSePuedeMover(char[,,] tablero, int fila, int columna, char colorJugador)
        {
            int filaInicio = -1;
            int columnaInicio = -1;
            bool impedirMovimiento = false;
            char[,,] tableroTemporal = new char[tablero.GetLength(0), tablero.GetLength(1), tablero.GetLength(2)];
            Array.Copy(tablero, tableroTemporal, tableroTemporal.Length);
            for (int i = 0; i < tablero.GetLongLength(0); i++) // Aquí solo tengo marcado como movimiento la pieza seleccionada
            {
                for (int j = 0; j < tablero.GetLength(1); j++)
                {
                    if (tableroTemporal[i, j, 1] == colorJugador)
                    {
                        if (tableroTemporal[i, j, 2] == movimiento || tableroTemporal[i, j, 2] == jaque)
                        {
                            filaInicio = i;
                            columnaInicio = j;
                        }
                    }
                }
            }
            for (int i = 0; i < tableroTemporal.GetLength(2) && (filaInicio > 0 || columnaInicio > 0); i++)
            {
                tableroTemporal[fila, columna, i] = tableroTemporal[filaInicio, columnaInicio, i];
                tableroTemporal[filaInicio, columnaInicio, i] = vacio;
            }
            ComprobarJaque(tableroTemporal);
            for (int i = 0; i < tableroTemporal.GetLength(0); i++)
            {
                for (int j = 0; j < tablero.GetLength(1); j++)
                {
                    if (tableroTemporal[i, j, 0] == rey && tableroTemporal[i, j, 1] == colorJugador &&
                       tableroTemporal[i, j, 2] == jaque)
                        impedirMovimiento = true;
                }
            }
            return impedirMovimiento;
        }
        ///////////////////////////////////// MOVIMIENTOS
        static (int fila, int columna) SeleccionarPieza(char[,,] tablero, char colorJugador)
        {
            bool piezaValida = false;
            int fila = -1;
            int columna = -1;
            Console.WriteLine("Pieza a mover");
            do
            {
                Console.Write("Fila: ");
                fila = PedirFila();
                Console.Write("Columna: ");
                columna = LetraColumnaANumero();
                if (fila < 0 || fila > 7 || columna < 0 || columna > 7)
                {
                    Console.WriteLine("Posición fuera del tablero");
                }
                else if (tablero[fila, columna, 1] == colorJugador)
                {
                    piezaValida = true;
                }
                else
                    Console.WriteLine("Pieza no válida");
            } while (piezaValida == false);
            if (tablero[fila, columna, 2] != jaque)
                tablero[fila, columna, 2] = movimiento;
            return (fila, columna);
        }
        static bool DeteccionGeneral(char[,,] tablero, int fila, int columna, char colorJugador, bool comprobarJaque = false)
        {
            bool impedirMovimiento = false;
            bool stop = false;
            switch (tablero[fila, columna, 1])
            {
                case vacio:
                    if (comprobarJaque == false)
                    {
                        impedirMovimiento = ComprobarSiSePuedeMover(tablero, fila, columna, colorJugador);
                        if (impedirMovimiento == false)
                            tablero[fila, columna, 2] = movimiento;
                    }
                    break;
                case magenta when colorJugador == magenta:
                    stop = true;
                    break;
                case magenta when colorJugador == verde:
                    if (tablero[fila, columna, 0] == rey && comprobarJaque == true)
                    {
                        tablero[fila, columna, 2] = jaque;
                    }
                    else if (comprobarJaque == false)
                    {
                        impedirMovimiento = ComprobarSiSePuedeMover(tablero, fila, columna, colorJugador);
                        if (impedirMovimiento == false)
                            tablero[fila, columna, 2] = movimiento;
                    }
                    stop = true;
                    break;
                case verde when colorJugador == magenta:
                    if (tablero[fila, columna, 0] == rey && comprobarJaque == true)
                    {
                        tablero[fila, columna, 2] = jaque;
                    }
                    else if (comprobarJaque == false)
                    {
                        impedirMovimiento = ComprobarSiSePuedeMover(tablero, fila, columna, colorJugador);
                        if (impedirMovimiento == false)
                            tablero[fila, columna, 2] = movimiento;
                    }
                    stop = true;
                    break;
                case verde when colorJugador == verde:
                    stop = true;
                    break;
            };
            return stop;
        }
        static bool RealizarMovimiento(char[,,] tablero, int fila, int columna)
        {
            bool jugadaRealizada = false;
            Console.WriteLine("Mover a:");
            Console.Write("Fila (0 para cancelar selección): ");
            int filaAMover = PedirFila();
            if (filaAMover >= 0 && filaAMover <= 7)
            {
                Console.Write("Columna(X para cancelar selección): ");
                int columnaAMover = LetraColumnaANumero();
                if (columnaAMover != -1 && (filaAMover != fila || columnaAMover != columna))
                {
                    if ((tablero[fila, columna, 0] == rey) && (columnaAMover == columna - 2)) // Enroque largo
                    {
                        for (int i = 0; i < tablero.GetLength(2); i++)
                        {
                            tablero[filaAMover, columnaAMover, i] = tablero[fila, columna, i];
                            tablero[fila, columna - 1, i] = tablero[fila, 0, i];
                            tablero[fila, columna, i] = vacio;
                            tablero[fila, 0, i] = vacio;
                        }
                    }
                    else if ((tablero[fila, columna, 0] == rey) && (columnaAMover == columna + 2)) // Enroque corto
                    {
                        for (int i = 0; i < tablero.GetLength(2); i++)
                        {
                            tablero[filaAMover, columnaAMover, i] = tablero[fila, columna, i];
                            tablero[fila, columna + 1, i] = tablero[fila, 7, i];
                            tablero[fila, columna, i] = vacio;
                            tablero[fila, 7, i] = vacio;
                        }
                    }
                    else if (tablero[filaAMover, columnaAMover, 2] == movimiento)
                    {
                        for (int i = 0; i < tablero.GetLength(2) - 1; i++)
                        {
                            tablero[filaAMover, columnaAMover, i] = tablero[fila, columna, i];
                            tablero[fila, columna, i] = vacio;
                        }
                        for (int i = 0; i < tablero.GetLength(0); i++)
                        {
                            if (tablero[0, i, 0] == peon || tablero[7, i, 0] == peon)
                                Promocion(tablero, filaAMover, i);
                        }
                        jugadaRealizada = true;
                    }
                }
                else
                {
                    Console.WriteLine("Movimiento cancelado.");
                    LimpiarPosiblesMovimientos(tablero);
                    ComprobarJaque(tablero);
                    // MostrarTablero(tablero);
                }
            }
            else
            {
                Console.WriteLine("Movimiento cancelado.");
                LimpiarPosiblesMovimientos(tablero);
                ComprobarJaque(tablero);
                // MostrarTablero(tablero);
            }
            return jugadaRealizada;
        }
        ///////////////////////////////////// PEON
        static bool DeteccionPeon(char[,,] tablero, int fila, int columna, char colorJugador)
        {
            bool impedirMovimiento = false;
            bool stop = false;
            switch (tablero[fila, columna, 1])
            {
                case vacio:
                    impedirMovimiento = ComprobarSiSePuedeMover(tablero, fila, columna, colorJugador);
                    if (impedirMovimiento == false)
                        tablero[fila, columna, 2] = movimiento;
                    break;
                case magenta:
                    stop = true;
                    break;
                case verde:
                    stop = true;
                    break;
            }
            return stop;
        }
        static void PosibleMovimientoPeon(char[,,] tablero, int fila, int columna, char colorJugador, bool comprobarJaque = false)
        {
            bool stop = false;
            bool impedirMovimiento = false;
            int cantidadMovimientoDisponible = 1;
            if ((colorJugador == verde && fila == 6) || (colorJugador == magenta && fila == 1))
                cantidadMovimientoDisponible = 2;
            if (colorJugador == verde)
            {
                for (int i = fila - 1; i >= fila - cantidadMovimientoDisponible && stop == false && comprobarJaque == false; i--)
                {
                    stop = DeteccionPeon(tablero, i, columna, colorJugador);
                }
                if (columna > 0)
                    if (tablero[fila - 1, columna - 1, 0] == rey && tablero[fila - 1, columna - 1, 1] == magenta && comprobarJaque == true)
                    {
                        tablero[fila - 1, columna - 1, 2] = jaque;
                        tablero[fila, columna, 2] = jaque;
                    }
                    else if (tablero[fila - 1, columna - 1, 1] == magenta && comprobarJaque == false)
                    {
                        impedirMovimiento = ComprobarSiSePuedeMover(tablero, fila - 1, columna - 1, colorJugador);
                        if (impedirMovimiento == false)
                            tablero[fila - 1, columna - 1, 2] = movimiento;
                    }
                if (columna < 7)
                    if (tablero[fila - 1, columna + 1, 0] == rey && tablero[fila - 1, columna + 1, 1] == magenta && comprobarJaque == true)
                    {
                        tablero[fila - 1, columna + 1, 2] = jaque;
                        tablero[fila, columna, 2] = jaque;
                    }
                    else if (tablero[fila - 1, columna + 1, 1] == magenta && comprobarJaque == false)
                    {
                        impedirMovimiento = ComprobarSiSePuedeMover(tablero, fila - 1, columna + 1, colorJugador);
                        if (impedirMovimiento == false)
                            tablero[fila - 1, columna + 1, 2] = movimiento;
                    }
            }
            if (colorJugador == magenta)
            {
                for (int i = fila + 1; i <= fila + cantidadMovimientoDisponible && stop == false && comprobarJaque == false; i++)
                {
                    stop = DeteccionPeon(tablero, i, columna, colorJugador);
                }
                if (columna > 0)
                    if (tablero[fila + 1, columna - 1, 0] == rey && tablero[fila + 1, columna - 1, 1] == verde && comprobarJaque == true)
                    {
                        tablero[fila + 1, columna - 1, 2] = jaque;
                        tablero[fila, columna, 2] = jaque;
                    }
                    else if (tablero[fila + 1, columna - 1, 1] == verde && comprobarJaque == false)
                    {
                        impedirMovimiento = ComprobarSiSePuedeMover(tablero, fila + 1, columna - 1, colorJugador);
                        if (impedirMovimiento == false)
                            tablero[fila + 1, columna - 1, 2] = movimiento;
                    }
                if (columna < 7)
                    if (tablero[fila + 1, columna + 1, 0] == rey && tablero[fila + 1, columna + 1, 1] == verde && comprobarJaque == true)
                    {
                        tablero[fila + 1, columna + 1, 2] = jaque;
                        tablero[fila, columna, 2] = jaque;
                    }
                    else if (tablero[fila + 1, columna + 1, 1] == verde && comprobarJaque == false)
                    {
                        impedirMovimiento = ComprobarSiSePuedeMover(tablero, fila + 1, columna + 1, colorJugador);
                        if (impedirMovimiento == false)
                            tablero[fila + 1, columna + 1, 2] = movimiento;
                    }
            }
        }
        ///////////////////////////////////// CABALLO
        static void PosibleMovimientoCaballo(char[,,] tablero, int fila, int columna, char colorJugador, bool comprobarJaque = false)
        {
            bool impedirMovimiento = false;
            for (int i = 0; i < tablero.GetLength(0); i++)
            {
                for (int j = 0; j < tablero.GetLength(1); j++)
                {
                    if ((Math.Abs(j - columna) == 2 && Math.Abs(i - fila) == 1) && (j != columna && i != fila)
                        ||
                        (Math.Abs(j - columna) == 1 && Math.Abs(i - fila) == 2) && (j != columna && i != fila))
                    {
                        switch (tablero[i, j, 1])
                        {
                            case vacio:
                                if (comprobarJaque == false)
                                {
                                    impedirMovimiento = ComprobarSiSePuedeMover(tablero, i, j, colorJugador);
                                    if (impedirMovimiento == false)
                                        tablero[i, j, 2] = movimiento;
                                }
                                break;
                            case magenta when colorJugador == magenta:
                                break;
                            case magenta when colorJugador == verde:
                                if (tablero[i, j, 0] == rey)
                                {
                                    tablero[i, j, 2] = jaque;
                                }
                                else if (comprobarJaque == false)
                                {
                                    impedirMovimiento = ComprobarSiSePuedeMover(tablero, i, j, colorJugador);
                                    if (impedirMovimiento == false)
                                        tablero[i, j, 2] = movimiento;
                                }
                                break;
                            case verde when colorJugador == magenta:
                                if (tablero[i, j, 0] == rey)
                                {
                                    tablero[i, j, 2] = jaque;
                                }
                                else if (comprobarJaque == false)
                                {
                                    impedirMovimiento = ComprobarSiSePuedeMover(tablero, i, j, colorJugador);
                                    if (impedirMovimiento == false)
                                        tablero[i, j, 2] = movimiento;
                                }
                                break;
                            case verde when colorJugador == verde:
                                break;
                        };
                    }
                }
            }
        }
        ///////////////////////////////////// ALFIL
        static void PosibleMovimientoAlfil(char[,,] tablero, int fila, int columna, char colorJugador, bool comprobarJaque = false)
        {
            bool stop = false;
            for (int i = fila - 1; i >= 0 && stop == false; i--) //Diagonal hacia [0,0]
            {
                for (int j = columna - 1; j >= 0 && stop == false; j--)
                {
                    if ((Math.Abs(j - columna) == Math.Abs(i - fila)) && (j != columna && i != fila))
                    {
                        stop = DeteccionGeneral(tablero, i, j, colorJugador, comprobarJaque);
                        if (tablero[i, j, 2] == jaque && tablero[i, j, 0] == rey)
                            tablero[fila, columna, 2] = jaque;
                    }
                }
            }
            stop = false;
            for (int i = fila - 1; i >= 0 && stop == false; i--) //Diagonal hacia [0,7]
            {
                for (int j = columna + 1; j < tablero.GetLength(1) && stop == false; j++)
                {
                    if ((Math.Abs(j - columna) == Math.Abs(i - fila)) && (j != columna && i != fila))
                    {
                        stop = DeteccionGeneral(tablero, i, j, colorJugador, comprobarJaque);
                        if (tablero[i, j, 2] == jaque && tablero[i, j, 0] == rey)
                            tablero[fila, columna, 2] = jaque;
                    }
                }
            }
            stop = false;
            for (int i = fila + 1; i < tablero.GetLength(0) && stop == false; i++) //Diagonal hacia [7,0]
            {
                for (int j = columna - 1; j >= 0 && stop == false; j--)
                {
                    if ((Math.Abs(j - columna) == Math.Abs(i - fila)) && (j != columna && i != fila))
                    {
                        stop = DeteccionGeneral(tablero, i, j, colorJugador, comprobarJaque);
                        if (tablero[i, j, 2] == jaque && tablero[i, j, 0] == rey)
                            tablero[fila, columna, 2] = jaque;
                    }
                }
            }
            stop = false;
            for (int i = fila + 1; i < tablero.GetLength(0) && stop == false; i++) //Diagonal hacia [7,7]
            {
                for (int j = columna + 1; j < tablero.GetLength(1) && stop == false; j++)
                {
                    if ((Math.Abs(j - columna) == Math.Abs(i - fila)) && (j != columna && i != fila))
                    {
                        stop = DeteccionGeneral(tablero, i, j, colorJugador, comprobarJaque);
                        if (tablero[i, j, 2] == jaque && tablero[i, j, 0] == rey)
                            tablero[fila, columna, 2] = jaque;
                    }
                }
            }
        }
        ///////////////////////////////////// TORRE
        static void PosibleMovimientoTorre(char[,,] tablero, int fila, int columna, char colorJugador, bool comprobarJaque = false)
        {
            bool stop = false;
            for (int i = fila - 1; i >= 0 && stop == false; i--) //Hacia arriba
            {
                stop = DeteccionGeneral(tablero, i, columna, colorJugador, comprobarJaque);
                if (tablero[i, columna, 2] == jaque && tablero[i, columna, 0] == rey)
                    tablero[fila, columna, 2] = jaque;
            }
            stop = false;
            for (int i = columna - 1; i >= 0 && stop == false; i--) //Hacia izquierda
            {
                stop = DeteccionGeneral(tablero, fila, i, colorJugador, comprobarJaque);
                if (tablero[i, columna, 2] == jaque && tablero[i, columna, 0] == rey)
                    tablero[fila, columna, 2] = jaque;
            }
            stop = false;
            for (int i = fila + 1; i < tablero.GetLength(1) && stop == false; i++) //Hacia abajo
            {
                stop = DeteccionGeneral(tablero, i, columna, colorJugador, comprobarJaque);
                if (tablero[i, columna, 2] == jaque && tablero[i, columna, 0] == rey)
                    tablero[fila, columna, 2] = jaque;
            }
            stop = false;
            for (int i = columna + 1; i < tablero.GetLength(0) && stop == false; i++) //Hacia derecha
            {
                stop = DeteccionGeneral(tablero, fila, i, colorJugador, comprobarJaque);
                if (tablero[i, columna, 2] == jaque && tablero[i, columna, 0] == rey)
                    tablero[fila, columna, 2] = jaque;
            }
        }
        ///////////////////////////////////// REY
        static void DeteccionRey(char[,,] tablero, int fila, int columna, char colorJugador)
        {
            bool impedirMovimiento = false;
            switch (tablero[fila, columna, 1])
            {
                case vacio:
                    impedirMovimiento = ComprobarSiSePuedeMover(tablero, fila, columna, colorJugador);
                    if (impedirMovimiento == false)
                        tablero[fila, columna, 2] = movimiento;
                    break;
                case magenta when colorJugador == magenta:
                    break;
                case magenta when colorJugador == verde:
                    impedirMovimiento = ComprobarSiSePuedeMover(tablero, fila, columna, colorJugador);
                    if (impedirMovimiento == false)
                        tablero[fila, columna, 2] = movimiento;
                    break;
                case verde when colorJugador == magenta:
                    impedirMovimiento = ComprobarSiSePuedeMover(tablero, fila, columna, colorJugador);
                    if (impedirMovimiento == false)
                        tablero[fila, columna, 2] = movimiento;
                    break;
                case verde when colorJugador == verde:
                    break;
            };
        }
        static Enrocar ComprobarEnroques(char[,,] tablero, Enrocar enroques)
        {
            if (tablero[0, 0, 0] != torre)
                enroques &= ~Enrocar.MagentaLargo;
            if (tablero[0, 7, 0] != torre)
                enroques &= ~Enrocar.MagentaCorto;
            if (tablero[7, 0, 0] != torre)
                enroques &= ~Enrocar.VerdeLargo;
            if (tablero[7, 7, 0] != torre)
                enroques &= ~Enrocar.VerdeCorto;
            if (tablero[0, 4, 0] != rey)
            {
                enroques &= ~Enrocar.MagentaLargo;
                enroques &= ~Enrocar.MagentaCorto;
            }
            if (tablero[7, 4, 0] != rey)
            {
                enroques &= ~Enrocar.VerdeLargo;
                enroques &= ~Enrocar.VerdeCorto;
            }
            return enroques;
        }
        static bool EnroqueLargo(char[,,] tablero, int fila, int columna)
        {
            bool enroque = true;
            if (tablero[fila, columna, 2] == jaque)
                enroque = false;
            for (int i = columna - 1; i > 1 && enroque == true; i--)
            {
                if (tablero[fila, i, 0] != vacio)
                    enroque = false;
            }
            return enroque;
        }
        static bool EnroqueCorto(char[,,] tablero, int fila, int columna)
        {
            bool enroque = true;
            if (tablero[fila, columna, 2] == jaque)
                enroque = false;
            for (int i = columna + 1; i < 7 && enroque == true; i++)
            {
                if (tablero[fila, i, 0] != vacio)
                    enroque = false;
            }
            return enroque;
        }
        static void PosibleMovimientoRey(char[,,] tablero, int fila, int columna, char colorJugador, Enrocar enroques)
        {
            bool impedirMovimiento = false;
            for (int i = 0; i < tablero.GetLength(0); i++)
            {
                for (int j = 0; j < tablero.GetLength(1); j++)
                {
                    if ((i >= fila - 1 && i <= fila + 1) && (j >= columna - 1 && j <= columna + 1))
                        DeteccionRey(tablero, i, j, colorJugador);
                }
            }
            if ((fila == 0 || fila == 7) && columna == 4)
            {
                if (colorJugador == magenta)
                {
                    if ((enroques & Enrocar.MagentaLargo) == Enrocar.MagentaLargo)
                    {
                        bool puedeEnrocar = EnroqueLargo(tablero, fila, columna);
                        if (puedeEnrocar == true)
                        {
                            impedirMovimiento = ComprobarSiSePuedeMover(tablero, fila, columna, colorJugador);
                            if (impedirMovimiento == false)
                                tablero[fila, columna - 2, 2] = movimiento;
                        }
                    }
                    if ((enroques & Enrocar.MagentaCorto) == Enrocar.MagentaCorto)
                    {
                        bool puedeEnrocar = EnroqueCorto(tablero, fila, columna);
                        if (puedeEnrocar == true)
                        {
                            impedirMovimiento = ComprobarSiSePuedeMover(tablero, fila, columna, colorJugador);
                            if (impedirMovimiento == false)
                                tablero[fila, columna + 2, 2] = movimiento;
                        }
                    }
                }
                if (colorJugador == verde)
                {
                    if ((enroques & Enrocar.VerdeLargo) == Enrocar.VerdeLargo)
                    {
                        bool puedeEnrocar = EnroqueLargo(tablero, fila, columna);
                        if (puedeEnrocar == true)
                        {
                            impedirMovimiento = ComprobarSiSePuedeMover(tablero, fila, columna, colorJugador);
                            if (impedirMovimiento == false)
                                tablero[fila, columna - 2, 2] = movimiento;
                        }
                    }
                    if ((enroques & Enrocar.VerdeCorto) == Enrocar.VerdeCorto)
                    {
                        bool puedeEnrocar = EnroqueCorto(tablero, fila, columna);
                        if (puedeEnrocar == true)
                        {
                            impedirMovimiento = ComprobarSiSePuedeMover(tablero, fila, columna, colorJugador);
                            if (impedirMovimiento == false)
                                tablero[fila, columna + 2, 2] = movimiento;
                        }
                    }
                }
            }
        }
        ///////////////////////////////////// TABLERO
        static void LimpiarPosiblesMovimientos(char[,,] tablero)
        {
            for (int i = 0; i < tablero.GetLength(0); i++)
            {
                for (int j = 0; j < tablero.GetLength(1); j++)
                {
                    tablero[i, j, 2] = vacio;
                }
            }
        }
        static void MostrarTablero(char[,,] tablero)
        {
            Console.WriteLine(" A B C D E F G H");
            for (int i = 0; i < tablero.GetLength(0); i++)
            {
                Console.Write($"{i + 1}");
                for (int j = 0; j < tablero.GetLength(1); j++)
                {
                    if (tablero[i, j, 2] == jaque)
                    {
                        Console.BackgroundColor = ConsoleColor.Yellow;
                    }
                    else if (tablero[i, j, 2] == movimiento)
                    {
                        Console.BackgroundColor = ConsoleColor.Red;
                    }
                    else if (i % 2 == 0 && j % 2 == 0 || i % 2 == 1 && j % 2 == 1)
                    {
                        Console.BackgroundColor = ConsoleColor.White;
                    }
                    if (tablero[i, j, 1] == magenta)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkMagenta;
                    }
                    else if (tablero[i, j, 1] == verde)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkGreen;
                    }
                    Console.Write($"{tablero[i, j, 0]} ");
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.ForegroundColor = ConsoleColor.White;
                }
                Console.WriteLine();
            }
        }
        static void Jugada(char[,,] tablero, char colorJugador, Enrocar enroques)
        {
            bool jugadaRealizada = false;
            do
            {
                (int fila, int columna) = SeleccionarPieza(tablero, colorJugador);
                switch (tablero[fila, columna, 0])
                {
                    case peon:
                        PosibleMovimientoPeon(tablero, fila, columna, colorJugador);
                        break;
                    case caballo:
                        PosibleMovimientoCaballo(tablero, fila, columna, colorJugador);
                        break;
                    case alfil:
                        PosibleMovimientoAlfil(tablero, fila, columna, colorJugador);
                        break;
                    case torre:
                        PosibleMovimientoTorre(tablero, fila, columna, colorJugador);
                        break;
                    case dama:
                        PosibleMovimientoAlfil(tablero, fila, columna, colorJugador);
                        PosibleMovimientoTorre(tablero, fila, columna, colorJugador);
                        break;
                    case rey:
                        PosibleMovimientoRey(tablero, fila, columna, colorJugador, enroques);
                        break;
                }
                MostrarTablero(tablero);
                jugadaRealizada = RealizarMovimiento(tablero, fila, columna);
                if (jugadaRealizada == false)
                {
                    LimpiarPosiblesMovimientos(tablero);
                    ComprobarJaque(tablero);
                    MostrarTablero(tablero);
                }
            } while (jugadaRealizada == false);
            MostrarTablero(tablero);
        }

        static void Main()
        {
            Console.Clear();
            Enrocar enroques = Enrocar.MagentaCorto | Enrocar.MagentaLargo | Enrocar.VerdeCorto | Enrocar.VerdeLargo;
            bool finPartida = false;
            char colorJugador = verde;
            char[,,] tablero = new char[8, 8, 3]
            {
                {{torre, magenta, vacio}, {caballo, magenta, vacio}, {alfil, magenta, vacio}, {dama, magenta, vacio}, {rey, magenta, vacio}, {alfil, magenta, vacio}, {caballo, magenta, vacio}, {torre, magenta, vacio}},
                {{peon, magenta, vacio}, {peon, magenta, vacio}, {peon, magenta, vacio}, {peon, magenta, vacio}, {peon, magenta, vacio}, {peon, magenta, vacio}, {peon, magenta, vacio}, {peon, magenta, vacio}},
                {{vacio, vacio, vacio}, {vacio, vacio, vacio}, {vacio, vacio, vacio}, {vacio, vacio, vacio}, {vacio, vacio, vacio}, {vacio, vacio, vacio}, {vacio, vacio, vacio}, {vacio, vacio, vacio}},
                {{vacio, vacio, vacio}, {vacio, vacio, vacio}, {vacio, vacio, vacio}, {vacio, vacio, vacio}, {vacio, vacio, vacio}, {vacio, vacio, vacio}, {vacio, vacio, vacio}, {vacio, vacio, vacio}},
                {{vacio, vacio, vacio}, {vacio, vacio, vacio}, {vacio, vacio, vacio}, {vacio, vacio, vacio}, {vacio, vacio, vacio}, {vacio, vacio, vacio}, {vacio, vacio, vacio}, {vacio, vacio, vacio}},
                {{vacio, vacio, vacio}, {vacio, vacio, vacio}, {vacio, vacio, vacio}, {vacio, vacio, vacio}, {vacio, vacio, vacio}, {vacio, vacio, vacio}, {vacio, vacio, vacio}, {vacio, vacio, vacio}},
                {{peon, verde, vacio}, {peon, verde, vacio}, {peon, verde, vacio}, {peon, verde, vacio}, {peon, verde, vacio}, {peon, verde, vacio}, {peon, verde, vacio}, {peon, verde, vacio}},
                {{torre, verde, vacio}, {caballo, verde, vacio}, {alfil, verde, vacio}, {dama, verde, vacio}, {rey, verde, vacio}, {alfil, verde, vacio}, {caballo, verde, vacio}, {torre, verde, vacio}}
            };
            MostrarTablero(tablero);
            do
            {
                Console.WriteLine($"Turno del jugador {(colorJugador == verde ? "verde" : "magenta")}");
                MostrarTablero(tablero);
                Jugada(tablero, colorJugador, enroques);
                ComprobarJaque(tablero);
                MostrarTablero(tablero);
                if (colorJugador == verde)
                    colorJugador = magenta;
                else
                    colorJugador = verde;
                enroques = ComprobarEnroques(tablero, enroques);
                finPartida = FinPartida(tablero, colorJugador, enroques);
                Console.Clear();
            } while (finPartida == false);
        }
    }
}