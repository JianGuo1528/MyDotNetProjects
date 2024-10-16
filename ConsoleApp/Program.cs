// See https://aka.ms/new-console-template for more information

var parkingSlots = new List<ParkingSlot>()
{
    new () {number = 1, Type = VehicleType.Small},
    new () {number = 2, Type = VehicleType.Small},
    new () {number = 3, Type = VehicleType.Regular},
    new () {number = 4, Type = VehicleType.Regular},
    new () {number = 5, Type = VehicleType.Regular},
    new () {number = 6, Type = VehicleType.Large},
    new () {number = 7, Type = VehicleType.Large},
    new () {number = 8, Type = VehicleType.Large}
};
var parkingLot = new ParkingLot(parkingSlots);
var m1 = new Motocycle();
var m2 = new Motocycle();
var m3 = new Motocycle();
var m4 = new Motocycle();
var m5 = new Motocycle();
var c1 = new Car();
var c2 = new Car();
var c3 = new Car();
parkingLot.ParkVehicle(m1);
parkingLot.ParkVehicle(m2);
parkingLot.ParkVehicle(m3);
parkingLot.ParkVehicle(m4);
parkingLot.ParkVehicle(m5);
parkingLot.ParkVehicle(c1);
parkingLot.ParkVehicle(c2);
parkingLot.ParkVehicle(c3);

Console.WriteLine($"Is parking lot full: {parkingLot.IsFull()}");

parkingLot.RemoveVehicle(m1);

Console.WriteLine($"Is parking lot full: {parkingLot.IsFull()}");

parkingLot.RemoveVehicle(m5);
Vehicle v1 = new Van();
var isParked = parkingLot.ParkVehicle(v1);
Console.WriteLine($"Can this van be parked: {isParked}");

foreach (var type in Enum.GetValues<VehicleType>())
{
    Console.WriteLine($"Is {type.ToString().ToLower()} parking slots full: {parkingLot.IsThisTypeFull(type)}");
}

public class ParkingLot
{
    public Dictionary<VehicleType, (HashSet<ParkingSlot>, HashSet<ParkingSlot>)> _parkingSlots { get; set; }
    public IDictionary<Vehicle, ParkingSlot> _vehiclesToParkingSlots { get; set; }

    public ParkingLot(List<ParkingSlot> parkingSlots)
    {
        _parkingSlots = new Dictionary<VehicleType, (HashSet<ParkingSlot>, HashSet<ParkingSlot>)>();
        var types = Enum.GetValues<VehicleType>();
        foreach (var type in types)
        {
            _parkingSlots[type] = ([], []);
        }
        foreach (var parkingSlot in parkingSlots)
        {
            var (availabilities, _) = _parkingSlots[parkingSlot.Type];
            availabilities.Add(parkingSlot);
        }

        _vehiclesToParkingSlots = new Dictionary<Vehicle, ParkingSlot>();
    }

    public bool ParkVehicle(Vehicle vehicle)
    {
        var parkingSlot = FindParkingLot(vehicle);
        if (parkingSlot != null)
        {
            parkingSlot.Vehicle = vehicle;
            return true;
        }

        return false;
    }

    public bool RemoveVehicle(Vehicle vehicle)
    {
        if (_vehiclesToParkingSlots.TryGetValue(vehicle, out var parkingSlot))
        {
            var (availabilities, unavailabilities) = _parkingSlots[parkingSlot.Type];
            availabilities.Add(parkingSlot);
            unavailabilities.Remove(parkingSlot);
            return true;
        }

        return false;
    }

    public bool IsFull()
    {
        var types = Enum.GetValues<VehicleType>();
        foreach (var type in types)
        {
            var isThisTypeFull = IsThisTypeFull(type);
            if (!isThisTypeFull) return false;
        }

        return true;
    }

    public bool IsEmpty()
    {
        var types = Enum.GetValues<VehicleType>();
        foreach (var type in types)
        {
            var isThisTypeEmpty = IsThisTypeEmpty(type);
            if (isThisTypeEmpty) return true;
        }

        return false;
    }

    public bool IsThisTypeFull(VehicleType type)
    {
        var (availabilities, _) = _parkingSlots[type];
        return availabilities.Count == 0;
    }

    public bool IsThisTypeEmpty(VehicleType type)
    {
        var (_, unavailabilities) = _parkingSlots[type];
        return unavailabilities.Count == 0;
    }

    private ParkingSlot FindParkingLot(Vehicle vehicle)
    {
        var types = Enum.GetValues<VehicleType>();
        foreach (var type in types)
        {
            if (type < vehicle.Type) continue;
            var (availabilities, unavailabilities) = _parkingSlots[type];
            if (availabilities.Count == 0) continue;
            var parkingSlot = availabilities.First();
            availabilities.Remove(parkingSlot);
            unavailabilities.Add(parkingSlot);
            _vehiclesToParkingSlots[vehicle] = parkingSlot;
            return parkingSlot;
        }

        return null;
    }
}

public enum VehicleType
{
    Small = 0,
    Regular = 1,
    Large = 2
}

public class ParkingSlot
{
    public Vehicle Vehicle { get; set; }
    public VehicleType Type { get; set; }
    public int number { get; set; }
}

public abstract class Vehicle
{
    public virtual VehicleType Type { get; }
    public string PlateNumber { get; set; }
}

public class Motocycle : Vehicle
{
    public override VehicleType Type { get; } = VehicleType.Small;
}

public class Car : Vehicle
{
    public override VehicleType Type { get; } = VehicleType.Regular;
}

public class Van : Vehicle
{
    public override VehicleType Type { get; } = VehicleType.Large;
}