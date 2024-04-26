const removeAuthKeys = () => {
	localStorage.removeItem("access_token");
	localStorage.removeItem("refresh_token");
	localStorage.removeItem("isLoggedIn");
  };
  
  export default removeAuthKeys;
  