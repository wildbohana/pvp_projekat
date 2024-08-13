// DriverDto - vraća se adminu kada pregleda sve vozače

export class Driver  {
    constructor(user) {
		this.email = user.email;
		this.username = user.username;
		this.firstname = user.firstname;
		this.lastname = user.lastname;
		this.verificationStatus = user.verificationStatus;
		this.isBlocked = user.isBlocked;
    }
}

export default Driver;
