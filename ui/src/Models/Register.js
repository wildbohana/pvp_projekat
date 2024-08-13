export class RegisterDto  {
    constructor(user) {
		this.email = user.email;
		this.username = user.username;
		this.password = user.password;
		this.confirmPassword = user.confirmPassword;
		this.firstname = user.firstname;
		this.lastname = user.lastname;
		this.address = user.address;
		this.dateOfBirth = user.dateOfBirth;
		this.role = user.role;
		this.photoUrl = user.photoUrl;
    }
}

export default RegisterDto;
