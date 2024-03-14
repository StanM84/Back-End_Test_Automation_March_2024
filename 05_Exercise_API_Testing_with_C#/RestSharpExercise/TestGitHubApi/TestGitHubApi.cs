using RestSharpServices;
using System.Net;
using System.Reflection.Emit;
using System.Text.Json;
using RestSharp;
using RestSharp.Authenticators;
using NUnit.Framework.Internal;
using RestSharpServices.Models;
using System;

namespace TestGitHubApi
{
    public class TestGitHubApi
    {
        private GitHubApiClient client;
        private static string repo;
        private static int lastCreatedIssueNumber;
        private static int lastCreatedCommentId;
        

        [SetUp]
        public void Setup()
        {            
            client = new GitHubApiClient("https://api.github.com/repos/testnakov/", "StanM84", "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");
            repo = "test-nakov-repo";
        }


        [Test, Order (1)]
        public void Test_GetAllIssuesFromARepo()
        {
            // Arrange

            // Act
            var issues = client.GetAllIssues(repo);

            // Assert
            Assert.That(issues, Has.Count.GreaterThan(0), "There should be more than one issue.");

            foreach (Issue issue in issues)
            {
                Assert.That(issue.Id , Is.GreaterThan(0), "Issue ID should be greater than 0.");
                Assert.That(issue.Number , Is.GreaterThan(0), "Issue Number should be greater than 0.");
                Assert.That(issue.Title , Is.Not.Empty, "Issue Title should not be empty.");
            }
        }

        [Test, Order (2)]
        public void Test_GetIssueByValidNumber()
        {
            // Arrange
            int issueNumber = 1; 

            // Act
            var issue = client.GetIssueByNumber(repo, issueNumber);

            // Assert
            Assert.That(issue, Is.Not.Null, "The response should contain issue data.");
            Assert.That(issue.Id, Is.GreaterThan(0), "Issue ID should be greater than 0.");
            Assert.That(issue.Number, Is.EqualTo(issueNumber), "Issue Number should be greater than 0.");
            Assert.That(issue.Title, Is.Not.Empty, "Issue Title should not be empty.");
        }
        
        [Test, Order (3)]
        public void Test_GetAllLabelsForIssue()
        {
            // Arrange
            int issueNumber = 6;

            // Act
            var labels = client.GetAllLabelsForIssue(repo, issueNumber);

            // Assert
            Assert.That(labels.Count, Is.GreaterThan(0));

            foreach (var lable in labels)
            {
                Assert.That(lable.Id, Is.GreaterThan(0), "Label ID shoud be mote than 0.");
                Assert.That(lable.Name, Is.Not.Null, "Label Name should not be null.");

                Console.WriteLine($"Label: {lable.Id} - Name: {lable.Name}");
            }
        }

        [Test, Order (4)]
        public void Test_GetAllCommentsForIssue()
        {
            // Arrange
            int issueNumber = 6;

            // Act
            var comments = client.GetAllCommentsForIssue(repo, issueNumber);

            // Assert
            Assert.That(comments.Count, Is.GreaterThan(0));

            foreach (var comment in comments)
            {
                Assert.That(comment.Id, Is.GreaterThan(0), "Comment ID shoud be mote than 0.");
                Assert.That(comment.Body, Is.Not.Null, "Comment Body should not be null.");
                Assert.That(comment.Body, Is.Not.Empty, "Comment Body should not be empty.");

                Console.WriteLine($"Label: {comment.Id} - Body: {comment.Body}");
            }
        }

        [Test, Order(5)]
        public void Test_CreateGitHubIssue()
        {
            // Arrange
            string title = "New issue title";
            string body = "New Body of the new Issue";


            // Act
            var issue = client.CreateIssue(repo, title, body);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(issue.Id, Is.GreaterThan(0));
                Assert.That(issue.Number, Is.GreaterThan(0));
                Assert.That(issue.Body, Is.EqualTo(body));
                Assert.That(issue.Title, Is.EqualTo(title));

            });
            Console.WriteLine(issue.Number);
            lastCreatedIssueNumber = issue.Number;
            
        }

        [Test, Order (6)]
        public void Test_CreateCommentOnGitHubIssue()
        {
            // Arrange
            int issueNumber = lastCreatedIssueNumber;
            string body = "New Body of the new Issue";


            // Act
            var comment = client.CreateCommentOnGitHubIssue(repo, issueNumber, body);

            // Assert
            Assert.That(comment.Body, Is.EqualTo(body));

            Console.WriteLine(comment.Id);
            lastCreatedCommentId = comment.Id;

        }

        [Test, Order (7)]
        public void Test_GetCommentById()
        {
            // Arrange
            int commentId = lastCreatedCommentId;

            // Act
            var comment = client.GetCommentById(repo, commentId);

            // Assert
            Assert.That(comment, Is.Not.Null);
            Assert.That(comment.Id, Is.EqualTo(commentId));
            Assert.That(comment.Body, Is.Not.Empty);

        }


        [Test, Order (8)]
        public void Test_EditCommentOnGitHubIssue()
        {
            // Arrange
            int commentId = lastCreatedCommentId;
            string newBody = "Edited Body";

            // Act
            var comment = client.EditCommentOnGitHubIssue(repo, commentId, newBody);

            // Assert
            Assert.That(comment, Is.Not.Null);
            Assert.That(comment.Id, Is.EqualTo(commentId));
            Assert.That(comment.Body, Is.EqualTo(newBody));
        }

        [Test, Order (9)]
        public void Test_DeleteCommentOnGitHubIssue()
        {
            // Arrange
            int commentId = lastCreatedCommentId;

            // Act
            var result = client.DeleteCommentOnGitHubIssue(repo, commentId);

            // Assert
            Assert.That(result, Is.True);
        }


    }
}

