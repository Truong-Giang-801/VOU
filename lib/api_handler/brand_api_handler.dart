import 'dart:convert';
import 'package:vou_web/class/user.dart';
import 'package:http/http.dart' as http;

class UserApiHandler {
  final String baseUri = "https://localhost:7001/api/auth/";

  Future<http.Response> getBrand({required User user}) async {
    final uri = Uri.parse(baseUri);
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
}
