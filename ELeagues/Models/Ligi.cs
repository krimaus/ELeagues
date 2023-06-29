using System;
using System.Collections.Generic;

namespace ELeagues.Models;

public partial class Ligi
{
    public int Idligi { get; set; }

    public int Idwlasciciela { get; set; }

    public virtual Uzytkownicy IdwlascicielaNavigation { get; set; } = null!;

    public virtual ICollection<Turnieje> Turniejes { get; set; } = new List<Turnieje>();
}
