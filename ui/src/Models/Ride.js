export class Ride {
    constructor(Ride) {
        this.id = Ride.id;
		this.startAddress = Ride.startAddress;
		this.finalAddress = Ride.finalAddress;
		this.distance = Ride.distance;
		this.price = Ride.price;
		this.pickUpTime = Ride.pickUpTime;
		this.rideDuration = Ride.rideDuration;
		this.startTime = Ride.startTime;
		this.status = Ride.status;
		this.customerId = Ride.customerId;
		this.driverId = Ride.driverId;
    }
}

export default Ride;
