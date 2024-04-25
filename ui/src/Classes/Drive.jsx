import React, {Component} from 'react';

class Drive extends Component {
	constructor(props) {
		super(props);
		this.state = {
			id: "testDrive"
		};
	}
	
	/*
	componentDidMount() {
		setTimeout(() => {
			this.setState({id: "newId"})
		}, 1000)
	}

	shouldComponentUpdate() {
    	return false;
  	}
	*/

	render() {
		return (
			<>
				<h2>Hi, I am a Drive!</h2>
				<h4>I have id: {this.state.id}!</h4>
			</>
		);
	}
}

export default Drive;
