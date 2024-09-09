import 'dart:convert';
import 'package:vou_web/class/user.dart';
import 'package:http/http.dart' as http;

final String baseUri = "https://localhost:7001/api/auth/";



Future<http.Response> register({required User user}) async {
  final uri = Uri.parse(baseUri + "register");
  late http.Response response;
  response = await http.post(
    uri,
    headers: <String, String>{
      'Content-type': 'application/json; charset=UTF-8',
    },
    body: json.encode({
      "name": user.name,
      "email": user.email,
      "phoneNumber": user.phoneNumber,
      "password": user.password,
      "roleName": user.role
    }),
  );

  return response;
}



Future<http.Response> assignRole({required User user}) async {
  final uri = Uri.parse(baseUri + "assignRole");
  late http.Response response;
  response = await http.post(
    uri,
    headers: <String, String>{
      'Content-type': 'application/json; charset=UTF-8',
    },
    body: json.encode({
      "name": user.name,
      "email": user.email,
      "phoneNumber": user.phoneNumber,
      "password": user.password,
      "roleName": user.role
    }),
  );

  return response;
}

Future<http.Response> login({required User user}) async {
  final uri = Uri.parse(baseUri + "login");
  late http.Response response;
  response = await http.post(
    uri,
    headers: <String, String>{
      'Content-type': 'application/json; charset=UTF-8',
    },
    body: json.encode({
      "username": user.email,
      "phoneNumber": user.phoneNumber,
      "password": user.password
    }),
  );

  return response;
}

Future<http.Response> createUser(String username, String email, String phoneNumber, String password, String roleName) async {
  final uri = Uri.parse('https://localhost:7001/api/auth/adduser');
  return await http.post(
    uri,
    headers: <String, String>{
      'Content-type': 'application/json; charset=UTF-8',
    },
    body: json.encode({
      "userName": username,
      "name": username,  // Include 'name' field to match your response
      "email": email,
      "phoneNumber": phoneNumber,
      "password": password,
      "roleName": roleName
    }),
  );
}


Future<http.Response> userDeActivate({required User user}) async {
  final uri = Uri.parse(baseUri + "activate-deactivate");
  late http.Response response;
  response = await http.put(
    uri,
    headers: <String, String>{
      'Content-type': 'application/json; charset=UTF-8',
    },
    body: json.encode(
        {"identifier": user.id, "isActive": user.lockout == "Activated"}),
  );

  return response;
}
Future<http.Response> updateUser({required User user}) async {
  final url = Uri.parse('https://localhost:7001/api/auth/update'); // Replace with your API endpoint
  final headers = {
    'Content-Type': 'application/json',
    // Replace with your auth token if needed
  };

  final body = json.encode({
    'id': user.id,
    'name': user.name,
    'email': user.email,
    'phoneNumber': user.phoneNumber,
    'password': user.password,
    'role': user.role,
    'lockout': user.lockout,
  });

  final response = await http.put(
    url,
    headers: headers,
    body: body,
  );

  return response;
}
Future<http.Response> deleteUser({required User user}) async {
  final uri = Uri.parse(baseUri + "user/" + user.id);
  late http.Response response;
  response = await http.delete(
    uri,
  );
  return response;
}

Future<List<User>> getUserData() async {
  List<User> data = [];
  final uri = Uri.parse(baseUri + "user");
  try {
    final response = await http.get(
      uri,
      headers: <String, String>{
        'Content-type': 'application/json; charset=UTF-8'
      },
    );
    if (response.statusCode >= 200 && response.statusCode <= 299) {
      final List<dynamic> jsonData = json.decode(response.body)['result'];
      data = jsonData.map((json) => User.fromJson(json)).toList();
    }
  } catch (e) {
    return data;
  }
  return data;
}

Future<User> getUserByEmail({required User user}) async {
  late User data = user;
  final uri = Uri.parse("${baseUri}user/email/${user.email}");
  try {
    final response = await http.get(
      uri,
      headers: <String, String>{
        'Content-type': 'application/json; charset=UTF-8'
      },
    );
    if (response.statusCode >= 200 && response.statusCode <= 299) {
      data = json.decode(response.body)['result'];
      return data;
    }
  } catch (e) {
    return data;
  }
  return data;
}
