import React, {Component} from 'react';

/*
The component will automatically re-render when its state changes, allowing you to easily build dynamic and responsive user interfaces. 
Props, on the other hand, are used to pass data from a parent component to a child component. 
This allows you to easily reuse components and customize their behavior based on the data passed to them.
*/

class User extends Component {
	constructor(props) {
		super(props);
		this.state = {
			id: "patientZero"
		};
	}

	render() {
		return (
			<>
				<h2>Hi, I am a User!</h2>
				<h4>I have id: {this.state.id}!</h4>
			</>
		);
	}
}

export default User;
