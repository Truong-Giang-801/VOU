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
