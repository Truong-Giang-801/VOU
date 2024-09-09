import 'package:flutter/material.dart';

class ReportPage extends StatelessWidget {
  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: Text('Báo cáo Thống kê'),
        backgroundColor: Colors.cyan,
      ),
      body: Padding(
        padding: const EdgeInsets.all(16.0),
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            Text(
              'Thống kê Dữ liệu Đối tác (Brand)',
              style: TextStyle(fontSize: 20, fontWeight: FontWeight.bold),
            ),
            // Add widgets to display brand statistics here
            SizedBox(height: 20),
            Text(
              'Thống kê Dữ liệu Người chơi',
              style: TextStyle(fontSize: 20, fontWeight: FontWeight.bold),
            ),
            // Add widgets to display player statistics here
            SizedBox(height: 20),
            Text(
              'Thống kê Dữ liệu Trò chơi',
              style: TextStyle(fontSize: 20, fontWeight: FontWeight.bold),
            ),
            // Add widgets to display game statistics here
          ],
        ),
      ),
    );
  }
}
