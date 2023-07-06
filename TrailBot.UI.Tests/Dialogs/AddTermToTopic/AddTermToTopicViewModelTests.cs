using CascadePass.TrailBot.UI.Dialogs.AddTermToTopic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CascadePass.TrailBot.UI.Tests.Dialogs.AddTermToTopic
{
    [TestClass]
    public class AddTermToTopicViewModelTests
    {
        [TestMethod]
        public void CanCreateInstance()
        {
            AddTermToTopicViewModel vm = new();
        }

        #region Property tests

        #region get/set access same value

        [TestMethod]
        public void Settings_GetSetAccessSameValue()
        {
            Settings settings = new();
            AddTermToTopicViewModel viewModel = new();

            viewModel.Settings = settings;
            Assert.AreSame(viewModel.Settings, settings);
        }

        [TestMethod]
        public void Topics_GetSetAccessSameValue()
        {
            List<Topic> topics = new();
            AddTermToTopicViewModel viewModel = new();

            viewModel.Topics = topics;
            Assert.AreSame(viewModel.Topics, topics);
        }

        [TestMethod]
        public void Topic_GetSetAccessSameValue()
        {
            Topic topic = new();
            AddTermToTopicViewModel viewModel = new();

            viewModel.Topic = topic;
            Assert.AreSame(viewModel.Topic, topic);
        }

        [TestMethod]
        public void SuggestAdditionalTerms_GetSetAccessSameValue()
        {
            AddTermToTopicViewModel viewModel = new() { Settings = new(), InitialTerm = "test" };
            bool value = !viewModel.SuggestAdditionalTerms;

            viewModel.SuggestAdditionalTerms = value;
            Assert.AreEqual(viewModel.SuggestAdditionalTerms, value);
        }

        [TestMethod]
        public void InitialTerm_GetSetAccessSameValue()
        {
            string value = Guid.NewGuid().ToString();
            AddTermToTopicViewModel viewModel = new();

            viewModel.InitialTerm = value;
            Assert.AreSame(viewModel.InitialTerm, value);
        }

        [TestMethod]
        public void AnyTerms_GetSetAccessSameValue()
        {
            string value = Guid.NewGuid().ToString();
            AddTermToTopicViewModel viewModel = new();

            viewModel.AnyTerms = value;
            Assert.AreSame(viewModel.AnyTerms, value);
        }

        [TestMethod]
        public void UnlessTerms_GetSetAccessSameValue()
        {
            string value = Guid.NewGuid().ToString();
            AddTermToTopicViewModel viewModel = new();

            viewModel.UnlessTerms = value;
            Assert.AreSame(viewModel.UnlessTerms, value);
        }

        [TestMethod]
        public void EditMode_GetSetAccessSameValue()
        {
            AddTermToTopicViewModel viewModel = new();
            AddTermMode value = viewModel.EditMode == AddTermMode.CreateNewTopic ? AddTermMode.AddToExistingTopic : AddTermMode.CreateNewTopic;

            viewModel.EditMode = value;
            Assert.AreEqual(viewModel.EditMode, value);
        }

        [TestMethod]
        public void Window_GetSetAccessSameValue()
        {
            AddTermToTopicViewModel viewModel = new();
            Window window = new();

            viewModel.Window = window;
            Assert.AreSame(viewModel.Window, window);
        }

        #endregion

        #region Title

        [TestMethod]
        public void Title_ValueNotSet()
        {
            AddTermToTopicViewModel viewModel = new() { EditMode = AddTermMode.ValueNotSet };
            Console.WriteLine($"{viewModel.EditMode}\t{viewModel.Title}");

            Assert.IsTrue(viewModel.Title.ToLower().Contains("bug"));
        }

        [TestMethod]
        public void Title_CreateNewTopic()
        {
            AddTermToTopicViewModel viewModel = new();

            viewModel.EditMode = AddTermMode.CreateNewTopic;
            Console.WriteLine($"{viewModel.EditMode}\t{viewModel.Title}");

            Assert.AreEqual(viewModel.Title, "Create a Topic");
        }

        [TestMethod]
        public void Title_AddToExistingTopic()
        {
            AddTermToTopicViewModel viewModel = new();

            viewModel.EditMode = AddTermMode.AddToExistingTopic;
            Console.WriteLine($"{viewModel.EditMode}\t{viewModel.Title}");

            Assert.AreEqual(viewModel.Title, "Add to Topic");
        }

        [TestMethod]
        public void Title_AddExceptionToExistingTopic()
        {
            AddTermToTopicViewModel viewModel = new();

            viewModel.EditMode = AddTermMode.AddExceptionToExistingTopic;
            Console.WriteLine($"{viewModel.EditMode}\t{viewModel.Title}");

            Assert.AreEqual(viewModel.Title, "Add to Topic");
        }

        #endregion

        #region CanAdd

        [TestMethod]
        public void CanAdd_NoTopic_False()
        {
            AddTermToTopicViewModel viewModel = new() { Topic = null };

            Assert.IsFalse(viewModel.CanAdd);
        }

        [TestMethod]
        public void CanAdd_UnnamedTopic_False()
        {
            AddTermToTopicViewModel viewModel = new() { Topic = new() { Name = null } };

            Assert.IsFalse(viewModel.CanAdd);
        }

        [TestMethod]
        public void CanAdd_NamedTopic_True()
        {
            AddTermToTopicViewModel viewModel = new() { Topic = new() { Name = Guid.NewGuid().ToString() } };

            Assert.IsTrue(viewModel.CanAdd);
        }

        #endregion

        #region Any/Unless Terms

        #region Not null by default

        [TestMethod]
        public void AnyTerms_DefaultsToEmptyString()
        {
            AddTermToTopicViewModel viewModel = new();

            Assert.AreEqual(string.Empty, viewModel.AnyTerms);
        }

        [TestMethod]
        public void UnlessTerms_DefaultsToEmptyString()
        {
            AddTermToTopicViewModel viewModel = new();

            Assert.AreEqual(string.Empty, viewModel.UnlessTerms);
        }

        #endregion

        #region Auto Trim on set

        [TestMethod]
        public void AnyTerms_TrimsOnSet()
        {
            string keyword = Guid.NewGuid().ToString();
            AddTermToTopicViewModel viewModel = new();

            viewModel.AnyTerms = $"\r\n   {keyword}\r\n\r\n";

            Console.WriteLine(viewModel.AnyTerms);
            Assert.AreEqual(keyword, viewModel.AnyTerms);
        }

        [TestMethod]
        public void UnlessTerms_TrimsOnSet()
        {
            string keyword = Guid.NewGuid().ToString();
            AddTermToTopicViewModel viewModel = new();

            viewModel.UnlessTerms = $"\r\n   {keyword}\r\n\r\n";

            Console.WriteLine(viewModel.UnlessTerms);
            Assert.AreEqual(keyword, viewModel.UnlessTerms);
        }

        #endregion

        #region Setting null doesn't throw

        [TestMethod]
        public void AnyTerms_SettingNullDoesNotThrow()
        {
            AddTermToTopicViewModel viewModel = new();
            viewModel.AnyTerms = null;
        }

        [TestMethod]
        public void UnlessTerms_SettingNullDoesNotThrow()
        {
            AddTermToTopicViewModel viewModel = new();
            viewModel.UnlessTerms = null;
        }

        #endregion

        #endregion

        #region InitialTerm

        [TestMethod]
        public void InitialTerm_Becomes_AnyTerms()
        {
            string term = Guid.NewGuid().ToString();
            AddTermToTopicViewModel viewModel = new()
            {
                EditMode = AddTermMode.AddToExistingTopic,
                InitialTerm = term,
            };

            Assert.AreEqual(term, viewModel.AnyTerms);
        }

        [TestMethod]
        public void InitialTerm_Becomes_UnlessTerms()
        {
            string term = Guid.NewGuid().ToString();
            AddTermToTopicViewModel viewModel = new()
            {
                EditMode = AddTermMode.AddExceptionToExistingTopic,
                InitialTerm = term,
            };

            Assert.AreEqual(term, viewModel.UnlessTerms);
        }

        #endregion

        [TestMethod]
        public void SuggestAdditionalTerms_DoesNotThrowWithoutInitialTerm()
        {
            AddTermToTopicViewModel viewModel = new() { Settings = new() };
            bool value = !viewModel.SuggestAdditionalTerms;

            viewModel.SuggestAdditionalTerms = value;
            Assert.AreEqual(viewModel.SuggestAdditionalTerms, value);
        }

        [TestMethod]
        public void SuggestAdditionalTerms_DoesNotThrowWithoutSettings()
        {
            AddTermToTopicViewModel viewModel = new();
            bool value = !viewModel.SuggestAdditionalTerms;

            viewModel.SuggestAdditionalTerms = value;

            // SuggestAdditionalTerms defers to Settings, which is null
            // in this test to validate that an exception will not be
            // thrown in this case.
        }

        #region Commands

        [TestMethod]
        public void AddCommand_NotNull()
        {
            AddTermToTopicViewModel viewModel = new();
            Assert.IsNotNull(viewModel.AddCommand);
        }

        [TestMethod]
        public void CancelCommand_NotNull()
        {
            AddTermToTopicViewModel viewModel = new();
            Assert.IsNotNull(viewModel.AddCommand);
        }

        [TestMethod]
        public void AddCommand_Not_CancelCommand()
        {
            AddTermToTopicViewModel viewModel = new();
            Assert.AreNotSame(viewModel.AddCommand, viewModel.CancelCommand);

            // The purpose of this test is to validate that the get accessors
            // of these two properties are not mixing things up and using
            // the same member variable.
        }

        #endregion

        #endregion

        #region Method tests

        #region Close

        [TestMethod]
        public void Close_NoWindow()
        {
            AddTermToTopicViewModel viewModel = new();
            viewModel.Close();

            // Expectation is there is no exception, a NRE seems likely here
        }

        [TestMethod]
        public void Close_WithWindow()
        {
            AddTermToTopicViewModel viewModel = new() { Window = new() };
            viewModel.Close();

            // Expectation is that no exception is thrown
        }

        #endregion

        #region Cancel

        [TestMethod]
        public void CanImmediatelyCancel()
        {
            AddTermToTopicViewModel viewModel = new();
            viewModel.CancelImplementation();

            // Expectation is that no exception is thrown
        }

        [TestMethod]
        public void Cancel_WasUpdated_False()
        {
            AddTermToTopicViewModel viewModel = new();
            viewModel.CancelImplementation();

            Assert.IsFalse(viewModel.WasUpdated);
        }

        [TestMethod]
        public void Cancel_NoChanges()
        {
            List<string> values = new();
            StringBuilder text = new();
            Topic topic = new() { MatchAny = string.Empty, MatchAnyUnless = string.Empty }; 
            AddTermToTopicViewModel viewModel = new() { Topic = topic };

            for (int i = 0; i < 10; i++)
            {
                values.Add(Guid.NewGuid().ToString());
                text.AppendLine(values[i]);
            }

            viewModel.AnyTerms = text.ToString();
            viewModel.UnlessTerms = text.ToString();

            viewModel.CancelImplementation();

            for (int i = 0; i < 10; i++)
            {
                Assert.IsFalse(topic.MatchAny.Contains(values[i]));
                Assert.IsFalse(topic.MatchAnyUnless.Contains(values[i]));
            }
        }

        #endregion

        #region Add

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void AddTermImplementation_RequiresTopic()
        {
            AddTermToTopicViewModel viewModel = new();

            viewModel.Topic = null;
            viewModel.AddTermImplementation();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void AddTermImplementation_RequiresValidEditMode()
        {
            AddTermToTopicViewModel viewModel = new();

            viewModel.Topic = new();
            viewModel.EditMode = AddTermMode.ValueNotSet;

            viewModel.AddTermImplementation();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void AddTermImplementation_RequiresCanAddToBeTrue()
        {
            AddTermToTopicViewModel viewModel = new();

            viewModel.Topic = new();
            viewModel.EditMode = AddTermMode.CreateNewTopic;

            viewModel.AddTermImplementation();
        }

        [TestMethod]
        public void AddTermImplementation_NoExceptionWithAllRequiredData()
        {
            AddTermToTopicViewModel viewModel = new();

            viewModel.Topic = new() { Name = Guid.NewGuid().ToString() };
            viewModel.EditMode = AddTermMode.CreateNewTopic;

            viewModel.AddTermImplementation();
        }

        [TestMethod]
        public void AddTermImplementation_UpdatesAny()
        {
            string keyword = Guid.NewGuid().ToString();
            Topic topic = new() { Name = keyword };
            AddTermToTopicViewModel viewModel = new() {
                Topic = topic,
                AnyTerms = keyword,
                EditMode = AddTermMode.AddToExistingTopic
            };

            viewModel.AddTermImplementation();

            Assert.IsTrue(topic.MatchAny.Contains(keyword));
        }

        [TestMethod]
        public void AddTermImplementation_UpdatesUnless()
        {
            string keyword = Guid.NewGuid().ToString();
            Topic topic = new() { Name = keyword };
            AddTermToTopicViewModel viewModel = new() {
                Topic = topic,
                UnlessTerms = keyword,
                EditMode = AddTermMode.AddToExistingTopic,
            };

            viewModel.AddTermImplementation();

            Assert.IsTrue(topic.MatchAnyUnless.Contains(keyword));
        }

        #endregion

        #region GetMergedText

        [TestMethod]
        public void GetMergedText_ReturnsSourceWhenTargetEmpty()
        {
            string source = Guid.NewGuid().ToString(), target = null;
            string result = AddTermToTopicViewModel.GetMergedText(source, target);

            Assert.AreSame(source, result);
        }

        [TestMethod]
        public void GetMergedText_ReturnsNewString()
        {
            string source = Guid.NewGuid().ToString(), target = Guid.NewGuid().ToString();
            string result = AddTermToTopicViewModel.GetMergedText(source, target);

            Assert.AreNotSame(source, result);
            Assert.AreNotSame(target, result);
        }

        [TestMethod]
        public void GetMergedText_TargetNotDuplicated()
        {
            string source = Guid.NewGuid().ToString(), target = source;
            string result = AddTermToTopicViewModel.GetMergedText(source, target);

            Console.WriteLine(result);
            Assert.IsTrue(string.Equals(result, target, StringComparison.OrdinalIgnoreCase));
        }

        [TestMethod]
        public void GetMergedText_EmptyLinesAreIgnored()
        {
            string
                source = $"{Guid.NewGuid()}\r\n\r\n\r\n{Guid.NewGuid()}\r\n\r\n{Guid.NewGuid()}\r\n",
                target = Guid.NewGuid().ToString();
            string result = AddTermToTopicViewModel.GetMergedText(source, target);

            Console.WriteLine(result);
            Assert.IsFalse(result.Contains("\r\n\r\n"));
        }

        [TestMethod]
        public void GetMergedText_ResultIsTrimmed()
        {
            string term = Guid.NewGuid().ToString();
            string
                source = $"{term}\r\n\r\n\r\n",
                target = term;
            string result = AddTermToTopicViewModel.GetMergedText(source, target);

            Console.WriteLine(result);
            Assert.IsTrue(string.Equals(result, term, StringComparison.OrdinalIgnoreCase));
        }

        [TestMethod]
        public void GetMergedText_IndividualTermsAreTrimmed()
        {
            string term = Guid.NewGuid().ToString();
            string
                source = $"{term}   \r\n\r\n\r\n",
                target = $"{term}\t";
            string result = AddTermToTopicViewModel.GetMergedText(source, target);

            Console.WriteLine(result);
            Assert.IsTrue(string.Equals(result, term, StringComparison.OrdinalIgnoreCase));
        }

        [TestMethod]
        public void GetMergedText_NullTargetDoesNotThrow()
        {
            string source = Guid.NewGuid().ToString(), target = null;
            string result = AddTermToTopicViewModel.GetMergedText(source, target);

            Assert.AreSame(source, result);
        }

        #endregion

        #region SplitIntoLines

        [TestMethod]
        public void SplitIntoLines_Null_NoException()
        {
            AddTermToTopicViewModel.SplitIntoLines(null);
        }

        [TestMethod]
        public void SplitIntoLines_TrimsEveryLine()
        {
            string text = $"{Guid.NewGuid()}\r\n{Guid.NewGuid()}   \r\n{Guid.NewGuid()}\r\n";
            string[] result = AddTermToTopicViewModel.SplitIntoLines(text);

            foreach (string line in result)
            {
                Assert.AreEqual(line, line.Trim());
            }
        }

        [TestMethod]
        public void SplitIntoLines_RemovesEmptyLines()
        {
            string text = $"{Guid.NewGuid()}\r\n{Guid.NewGuid()}\r\n\r\n\r\n   \r\n{Guid.NewGuid()}\r\n";
            string[] result = AddTermToTopicViewModel.SplitIntoLines(text);

            foreach (string line in result)
            {
                Assert.IsFalse(string.IsNullOrWhiteSpace(line));
            }
        }

        #endregion

        #region GetSuggestions

        [TestMethod]
        public void GetSuggestions_Null_DoesNotThrow()
        {
            AddTermToTopicViewModel.GetSuggestions(null);

            // Just making it here demonstrates that a null input does not throw an exception
        }

        [TestMethod]
        public void GetSuggestions_ReturnsLargerString()
        {
            string text = Guid.NewGuid().ToString();
            string result = AddTermToTopicViewModel.GetSuggestions(text);

            Assert.IsTrue(result.Length > text.Length);
        }

        [TestMethod]
        public void GetSuggestions_ReturnsExpectedResult()
        {
            string text = "ski";
            string result = AddTermToTopicViewModel.GetSuggestions(text);

            string expected = "skis\r\nskiing\r\nskier\r\nskiers";

            Console.WriteLine($"Expected: {expected}");
            Console.WriteLine($"Result: {result}");
            Assert.IsTrue(string.Equals(expected, result, StringComparison.OrdinalIgnoreCase));
        }

        [TestMethod]
        public void GetSuggestions_DoesNotRepeatPostfixes()
        {
            string text = "breaking";
            string result = AddTermToTopicViewModel.GetSuggestions(text);

            Console.WriteLine($"Result:\r\n{result}");
            Assert.IsFalse(("\r\n" + result + "\r\n").Contains("\r\nbreaking\r\n"));
        }

        #endregion

        #region AddSuggestions

        #region Validate the correct property is being used

        [TestMethod]
        public void AddSuggestions_MatchAny()
        {
            string keyword = Guid.NewGuid().ToString();
            AddTermToTopicViewModel viewModel = new() {
                EditMode = AddTermMode.AddToExistingTopic,
            };

            viewModel.AddSuggestions(keyword);

            Assert.IsTrue(viewModel.AnyTerms.Contains(keyword));
            Assert.IsFalse(viewModel.UnlessTerms.Contains(keyword));
        }

        [TestMethod]
        public void AddSuggestions_MatchUnless()
        {
            string keyword = Guid.NewGuid().ToString();
            AddTermToTopicViewModel viewModel = new()
            {
                EditMode = AddTermMode.AddExceptionToExistingTopic,
            };

            viewModel.AddSuggestions(keyword);

            Assert.IsFalse(viewModel.AnyTerms.Contains(keyword));
            Assert.IsTrue(viewModel.UnlessTerms.Contains(keyword));
        }

        #endregion

        #endregion

        #region RemoveSuggestions

        [TestMethod]
        public void RemoveSuggestions_null_NoException()
        {
            AddTermToTopicViewModel viewModel = new();
            viewModel.RemoveSuggestions(null);
        }

        [TestMethod]
        public void RemoveSuggestions_RemovesPostfixes_Any()
        {
            AddTermToTopicViewModel viewModel = new();
            viewModel.EditMode = AddTermMode.AddToExistingTopic;
            viewModel.AnyTerms = "skis\r\nskiing\r\nskier\r\nskiers";
            viewModel.RemoveSuggestions("ski");

            Console.WriteLine(viewModel.AnyTerms);
            Assert.IsTrue(string.IsNullOrEmpty(viewModel.AnyTerms));
        }

        [TestMethod]
        public void RemoveSuggestions_RemovesPostfixes_Unless()
        {
            AddTermToTopicViewModel viewModel = new();
            viewModel.EditMode = AddTermMode.AddExceptionToExistingTopic;
            viewModel.UnlessTerms = "skis\r\nskiing\r\nskier\r\nskiers";
            viewModel.RemoveSuggestions("ski");

            Console.WriteLine(viewModel.UnlessTerms);
            Assert.IsTrue(string.IsNullOrEmpty(viewModel.UnlessTerms));
        }

        [TestMethod]
        public void RemoveSuggestions_PreservesOtherTerms_Any()
        {
            AddTermToTopicViewModel viewModel = new();
            viewModel.EditMode = AddTermMode.AddToExistingTopic;
            viewModel.AnyTerms = "parking\r\nskis\r\nskiing\r\nskier\r\nskiers";
            viewModel.RemoveSuggestions("ski");

            Console.WriteLine(viewModel.AnyTerms);
            Assert.AreEqual("parking", viewModel.AnyTerms);
        }

        #endregion

        #endregion
    }
}
