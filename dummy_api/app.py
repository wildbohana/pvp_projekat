"""
# Flask venv setup (python 3.11)
# https://dev.to/mursalfk/setup-flask-on-windows-system-using-vs-code-4p9j

mkdir dummy_api
cd dummy_api
python -m venv venv
venv\Scripts\activate
pip install flask flask_restful
[kopirati ovaj fajl u folder]
flask run
"""


from flask import Flask
from flask_restful import Api, reqparse, Resource


app = Flask(__name__)
api = Api(app)


# Base route
@app.route('/')
def index():
    return 'Hello, World!'


# Login
userLoginArgs = reqparse.RequestParser()
userLoginArgs.add_argument("email", type=str, help="Email is required", required=True)
userLoginArgs.add_argument("password", type=str, help="Password is required", required=True)

class Login(Resource):
    def post(self):
        try:
            args = userLoginArgs.parse_args()
            print("[POST] Login request")
            print(args['email'])
            print(args['password'])
            return "All okay!", 200
        except Exception as e:
            return "Error: " + str(e), 500

api.add_resource(Login, "/auth/login")


# Register
# Ovo su za sad tipovi string, kasnije naravno menjati na DateTime itd.
userRegisterArgs = reqparse.RequestParser()
userRegisterArgs.add_argument("email", type=str, help="Email is required", required=True)
userRegisterArgs.add_argument("password", type=str, help="Password is required", required=True)
userRegisterArgs.add_argument("username", type=str, help="Username is required", required=True)
userRegisterArgs.add_argument("firstname", type=str, help="Name is required", required=True)
userRegisterArgs.add_argument("lastname", type=str, help="Surname is required", required=True)
userRegisterArgs.add_argument("dob", type=str, help="Date of birth is required", required=True)
userRegisterArgs.add_argument("address", type=str, help="Address is required", required=True)
userRegisterArgs.add_argument("usertype", type=str, help="User type (admin/driver/customer) is required", required=True)
#userRegisterArgs.add_argument("image", type=str, help="Image is required", required=True)

class Register(Resource):
    def post(self):
        try:
            args = userRegisterArgs.parse_args()
            print("[POST] Register request")
            print(args['email'])
            print(args['password'])
            print(args['username'])
            print(args['firstname'])
            print(args['lastname'])
            print(args['dob'])
            print(args['address'])
            print(args['usertype'])
            #print(args['image'])
            return "All okay!", 200
        except Exception as e:
            return "Error: " + str(e), 500

api.add_resource(Register, "/auth/register")


# Izmena profila
userUpdateArgs = reqparse.RequestParser()
userUpdateArgs.add_argument("email", type=str)
userUpdateArgs.add_argument("password", type=str)
#userUpdateArgs.add_argument("username", type=str)
userUpdateArgs.add_argument("firstname", type=str)
userUpdateArgs.add_argument("lastname", type=str)
userUpdateArgs.add_argument("dob", type=str)
userUpdateArgs.add_argument("address", type=str)
#userUpdateArgs.add_argument("usertype", type=str)

class ProfileUpdate(Resource):
    def patch(self, token):
        try:
            args = userUpdateArgs.parse_args()
            print("[PATCH] User profile request")
            print(token)
            print(args['email'])
            print(args['password'])
            print(args['firstname'])
            print(args['lastname'])
            print(args['dob'])
            print(args['address'])
            return "All okay!", 200
        except Exception as e:
            return "Error: " + str(e), 500

# Po konvenciji bi trebao ID da se koristi, ali valjda može i ovo
api.add_resource(ProfileUpdate, "/profile/update/<string:token>")


# Prikaz profila
class ProfileShow(Resource):
    def get(self, username):
        try:
            #args = userUpdateArgs.parse_args()
            print("[GET] User profile request")
            print(username)
            #TODO vrati neki profil
            return "All okay!", 200
        except Exception as e:
            return "Error: " + str(e), 500

# Koristim username umesto ID (svakako je username unique)
api.add_resource(ProfileShow, "/profile/show/<string:username>")


# Nova vožnja (/ride/new)
newRideArgs = reqparse.RequestParser()
newRideArgs.add_argument("token", type=str)
newRideArgs.add_argument("start", type=str)
newRideArgs.add_argument("end", type=str)

class NewRide(Resource):
    def post(self):
        try:
            args = newRideArgs.parse_args()
            print("[POST] New ride")
            print(args['token'])
            print(args['start'])
            print(args['end'])
            # U C# staviti status na PENDING
            # Takođe ocenu vožnje staviti na 0 (ili -1)
            #TODO vrati predviđenu cenu i vreme za koji će vozač stići
            return "All okay!", 200
        except Exception as e:
            return "Error: " + str(e), 500

api.add_resource(NewRide, "/ride/new")


# Prethodne vožnje (/ride/previous/{user-id})
# Staviti da prima token kao args?
class PreviousRides(Resource):
    def get(self, username):
        try:
            #args = userUpdateArgs.parse_args()
            print("[GET] Previous rides for username: " + username)
            #TODO vrati neku listu
            return "All okay!", 200
        except Exception as e:
            return "Error: " + str(e), 500

# Koristim username umesto ID (svakako je username unique)
api.add_resource(PreviousRides, "/ride/previous/<string:username>")


# Potvrda vožnje (/ride/accept/{ride-id})
acceptRideArgs = reqparse.RequestParser()
acceptRideArgs.add_argument("token", type=str)

class AcceptRide(Resource):
    def patch(self, id):
        try:
            args = acceptRideArgs.parse_args()
            print("[PATCH] Accept ride with ID: " + id)
            print("Token: " + args['token'])
            return "All okay!", 200
        except Exception as e:
            return "Error: " + str(e), 500

# ID vožnje
api.add_resource(AcceptRide, "/ride/accept/<string:id>")


# Odbijanje vožnje (/ride/deny/{ride-id})
denyRideArgs = reqparse.RequestParser()
denyRideArgs.add_argument("token", type=str)

class DenyRide(Resource):
    def patch(self, id):
        try:
            args = denyRideArgs.parse_args()
            print("[PATCH] Deny ride with ID: " + id)
            print("Token: " + args['token'])
            return "All okay!", 200
        except Exception as e:
            return "Error: " + str(e), 500

# ID vožnje, ne korisnika lol
api.add_resource(DenyRide, "/ride/accept/<string:id>")


# Provera da li korisnik trenutno ima aktivnu vožnju (/ride/active-ride-check/{user-id})
class ActiveRideCheck(Resource):
    def get(self, username):
        try:
            print("[GET] Check if user has active ride")
            print(username)
            #TODO vrati neki bool
            return "All okay!", 200
        except Exception as e:
            return "Error: " + str(e), 500

# Koristim username umesto ID (svakako je username unique)
api.add_resource(ActiveRideCheck, "/ride/active-ride-check/<string:username>")


# Oceni vožnju (/ride/rate/{iride-d})
rateRideArgs = reqparse.RequestParser()
rateRideArgs.add_argument("token", type=str)
rateRideArgs.add_argument("rating", type=int)

class RateRide(Resource):
    def patch(self, id):
        try:
            # Iz tokena se izvlači ID korisnika
            args = rateRideArgs.parse_args()
            print("[PATCH] Rate ride with ID: " + id)
            print("Token: " + args['token'])
            print("Rating: " + args['rating'])
            return "All okay!", 200
        except Exception as e:
            return "Error: " + str(e), 500

api.add_resource(RateRide, "/ride/rate/<string:id>")


# Da li je vozač trenutno blokiran (/driver/admin-block-check/{driver-id})
class DriverBlockedCheck(Resource):
    def get(self, username):
        try:
            print("[GET] Check if driver has been blocked")
            print(username)
            #TODO vrati neki bool
            return "All okay!", 200
        except Exception as e:
            return "Error: " + str(e), 500

# Username je driver-id
api.add_resource(DriverBlockedCheck, "/driver/admin-block-check/<string:username>")


# Svi vozači (admin) (/driver/all-drivers)
# Staviti da prima token kao args ?
# Ili ostviti frontu da brine da li je trenutni korisnik admin
class AllDrivers(Resource):
    def get(self):
        try:
            print("[GET] All drivers")
            #TODO vrati neku listu
            return "All okay!", 200
        except Exception as e:
            return "Error: " + str(e), 500

# Koristim username umesto ID (svakako je username unique)
api.add_resource(AllDrivers, "/driver/all-drivers")


# Nove vožnje (vozać) (/ride/pending)
class PendingRides(Resource):
    def get(self):
        try:
            print("[GET] Pending drives")
            #TODO vrati neku listu
            return "All okay!", 200
        except Exception as e:
            return "Error: " + str(e), 500

# Koristim username umesto ID (svakako je username unique)
api.add_resource(PendingRides, "/ride/pending")


# Sve vožnje (admin) (/ride/all-rides)
# Staviti da prima token kao args ?
# Ili ostviti frontu da brine da li je trenutni korisnik admin
class AllRidesEver(Resource):
    def get(self):
        try:
            print("[GET] All drives ever")
            #TODO vrati neku listu
            return "All okay!", 200
        except Exception as e:
            return "Error: " + str(e), 500

# Koristim username umesto ID (svakako je username unique)
api.add_resource(AllRidesEver, "/ride/all-rides")


# Prosečna ocena vozača (admin) (/driver/average-score/{id-vozaca})
# Staviti da prima token kao args ?
# Ili ostviti frontu da brine da li je trenutni korisnik admin
class DriverAverageScore(Resource):
    def get(self, username):
        try:
            print("[GET] Driver's average score")
            print("Driver username: " + username)
            #TODO vrati neku listu
            return "All okay!", 200
        except Exception as e:
            return "Error: " + str(e), 500

# Username vozača
api.add_resource(DriverAverageScore, "/driver/average-score/<string:username>")


# Blokiraj vozača (admin) (/driver/block-driver/{id})
class BlockDriver(Resource):
    def patch(self, username):
        try:
            print("[PATCH] Block driver with username: " + username)
            return "All okay!", 200
        except Exception as e:
            return "Error: " + str(e), 500

# ID vožnje
api.add_resource(BlockDriver, "/driver/block-driver/<string:username>")


# Odblokiraj vozača (admin) (/driver/unblock-driver/{id})
class UnBlockDriver(Resource):
    def patch(self, username):
        try:
            print("[PATCH] Unblock driver with username: " + username)
            return "All okay!", 200
        except Exception as e:
            return "Error: " + str(e), 500

# ID vožnje
api.add_resource(UnBlockDriver, "/driver/unblock-driver/<string:username>")



# TODO
# Verifikacija od admina (/admin/verification/{id})
# Ovo još uvek nisam smislila kako


# main driver function
if __name__ == '__main__':
    app.run()
