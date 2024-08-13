export class User  {
    constructor(user) {
		this.email = user.email;
		this.username = user.username;
		this.confirmOldPassword = user.confirmOldPassword;
		this.newPassword = user.newPassword;
		this.confirmNewPassword = user.confirmNewPassword;
		this.firstname = user.firstname;
		this.lastname = user.lastname;
		this.address = user.address;
		this.dateOfBirth = user.dateOfBirth;
		this.role = user.role;
		this.photoUrl = user.photoUrl;
    }
}

export default User;
