const express = require('express');
const app = express();
const PORT = 3001;
const cors = require('cors');
app.use(express.urlencoded({ extended: true }));
app.use(express.json());

app.use(cors());
app.get('/getGameInfo', (req, res) => {
  console.log('GET');
  res.json({
    testId: '123',
    userId: 'testUnityWebReq',
    gameName: 'testGameName',
    questions: [
      {
        questionDescription: 'What is the result of 2 + 3: ?',
        answers: [0, 2, 4, 5],
      },
      {
        questionDescription: 'What is the result of 2 + 4: ?',
        answers: [0, 2, 6, 5],
      },
      {
        questionDescription: 'What is the result of 2 + 5: ?',
        answers: [0, 2, 7, 5],
      },
    ],
  });
});

app.post('/submitTest', (req, res) => {
  let rs = ['D', 'C', 'C'];
  const { answers, testId, userId } = req.body;
  console.log(req.body);

  let score = 0;

  rs.forEach((item, idx) => {
    if (item === answers[idx]) {
      score++;
    }
  });

  res.json({
    testId,
    userId,
    score: score,
  });
});

app.listen(PORT, () => {
  console.log(`Server is on port ${PORT}`);
});
